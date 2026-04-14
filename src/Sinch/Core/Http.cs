using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Auth;
using Sinch.Logger;

namespace Sinch.Core
{
    /// <summary>
    ///     A single place to control token fetching and common headers.
    /// </summary>
    internal interface IHttp
    {
        /// <summary>
        ///     Use to send http request without a body
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="headers"></param>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null);

        Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod, HttpContent content,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Use to send http request with a body
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="headers"></param>
        /// <typeparam name="TRequest">The type of the request object.</typeparam>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest httpContent,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null);

        JsonSerializerOptions JsonSerializerOptions { get; }
    }

    /// <summary>
    ///     Represents an empty response for cases where no json is expected.
    /// </summary>
    public sealed class EmptyResponse
    {
    }

    /// <inheritdoc /> 
    internal sealed class Http : IHttp
    {
        private readonly Func<HttpClient> _httpClientAccessor;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILoggerAdapter<IHttp>? _logger;
        private readonly Lazy<ISinchAuth> _auth;

        /// <summary>
        ///     Gets the User-Agent header value for HTTP requests.
        /// </summary>
        internal static string UserAgent { get; } = BuildUserAgent();

        public Http(Lazy<ISinchAuth> auth, Func<HttpClient> httpClientAccessor, ILoggerAdapter<IHttp>? logger,
            JsonNamingPolicy jsonNamingPolicy)
        {
            _logger = logger;
            _auth = auth;
            _httpClientAccessor = httpClientAccessor;
            _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = jsonNamingPolicy,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { SkipUnsetOptionalProperties }
                }
            };
        }

        /// <summary>
        ///     <see cref="DefaultJsonTypeInfoResolver" /> modifier that prevents STJ from writing the
        ///     JSON property name for any <see cref="Optional{T}" /> property that is in the
        ///     <see cref="Optional{T}.Unset" /> state.  Without this, STJ writes the property name
        ///     before calling the converter's <c>Write</c> method, producing malformed JSON
        ///     (<c>"field":</c> with no value) when the converter writes nothing.
        /// </summary>
        private static void SkipUnsetOptionalProperties(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Kind != JsonTypeInfoKind.Object) return;
            foreach (var property in typeInfo.Properties)
            {
                var pt = property.PropertyType;
                if (!pt.IsGenericType || pt.GetGenericTypeDefinition() != typeof(Optional<>)) continue;
                property.ShouldSerialize = static (_, val) => val is not IOptional opt || !opt.IsUnset;
            }
        }

        public Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod, HttpContent content,
            CancellationToken cancellationToken = default)
        {
            return SendHttpContent<TResponse>(uri, httpMethod, content, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null)
        {
            return Send<EmptyResponse, TResponse>(uri, httpMethod, null, cancellationToken, headers);
        }

        private async Task<TResponse> SendHttpContent<TResponse>(Uri uri, HttpMethod httpMethod,
            HttpContent? httpContent,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null)
        {
            var retry = true;
            while (true)
            {
                _logger?.LogDebug("Sending request to {uri}", uri);

#if DEBUG
                Debug.WriteLine($"Http Method: {httpMethod}");
                Debug.WriteLine($"Request uri: {uri}");
#endif

                using var msg = new HttpRequestMessage();
                msg.RequestUri = uri;
                msg.Method = httpMethod;
                msg.Content = httpContent;

                (var token, retry) = await Authenticate(msg, retry, cancellationToken);

                msg.Headers.Authorization = new AuthenticationHeaderValue(_auth.Value.Scheme, token);

                msg.Headers.Add("User-Agent", UserAgent);

                if (headers != null && headers.Any())
                {
                    AddOrOverrideHeaders(msg, headers);
                }

                // Get HttpClient from factory/accessor - do not dispose as factory manages lifetime
                var httpClient = _httpClientAccessor();
                var result = await httpClient.SendAsync(msg, cancellationToken);

                if (result.StatusCode == HttpStatusCode.Unauthorized && retry)
                {
                    // will not retry when no "expired" header for a token.
                    const string wwwAuthenticateHeader = "www-authenticate";
                    if (_auth.Value.Scheme == AuthSchemes.Bearer && (!result.Headers.Contains(wwwAuthenticateHeader) ||
                        !result.Headers.GetValues(wwwAuthenticateHeader).Any(x => x.Contains("expired"))))
                    {
                        _logger?.LogDebug("OAuth Unauthorized");
                    }
                    else
                    {
                        retry = false;
                        continue;
                    }
                }

                await result.EnsureSuccessApiStatusCode(_jsonSerializerOptions);

                _logger?.LogDebug("Finished processing request for {uri}", uri);

#if DEBUG
                try
                {
                    var responseStr = await result.Content.ReadAsStringAsync(cancellationToken);
                    Debug.WriteLine($"Response string: {responseStr}");
                    using var jDoc = JsonDocument.Parse(responseStr);
                    Debug.WriteLine(
                        $"Response content: {JsonSerializer.Serialize(jDoc, new JsonSerializerOptions() { WriteIndented = true })}");
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to parse json {e.Message}");
                }
#endif

                // if empty response is expected, any non-related response is dropped
                if (typeof(TResponse) == typeof(EmptyResponse))
                {
                    // if not empty content, check what is there for debug purposes.
                    // C# EmptyContent class is internal, so checking it by the name
                    // for more details, see: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Net.Http/src/System/Net/Http/EmptyContent.cs
                    if (result.Content.GetType().Name != "EmptyContent")
                    {
                        _logger?.LogDebug("Expected empty content, but got {content}",
                            await result.Content.ReadAsStringAsync(cancellationToken));
                    }

                    return (TResponse)(object)new EmptyResponse();
                }

                // NOTE: there wil probably be other files supported in the future
                if (result.IsPdf())
                {
                    if (typeof(TResponse) != typeof(ContentResult))
                    {
                        throw new InvalidOperationException(
                            $"Received pdf, but expected response type is not a {nameof(ContentResult)}.");
                    }

                    // yes, the header currently returns double quotes ""IFOFJSLJ12313.pdf""
                    var fileName = result.Content.Headers.ContentDisposition?.FileName?.Trim('"');
                    return (TResponse)(object)new ContentResult()
                    {
                        Stream = await result.Content.ReadAsStreamAsync(cancellationToken),
                        FileName = fileName
                    };
                }



                if (result.IsJson())
                    return await result.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken,
                               options: _jsonSerializerOptions)
                           ?? throw new InvalidOperationException(
                               $"{typeof(TResponse).Name} is null");

                // unexpected content, log warning and throw exception
                _logger?.LogWarning("Response is not json, but {content}",
                    await result.Content.ReadAsStringAsync(cancellationToken));

                throw new InvalidOperationException("The response is not Json or EmptyResponse");
            }
        }

        public JsonSerializerOptions JsonSerializerOptions => _jsonSerializerOptions;

        private static void AddOrOverrideHeaders(HttpRequestMessage msg,
            Dictionary<string, IEnumerable<string>> headers)
        {
            foreach (var header in headers)
            {
                if (msg.Headers.Contains(header.Key))
                {
                    msg.Headers.Remove(header.Key);
                }

                msg.Headers.Add(header.Key, header.Value);
            }
        }

        private async Task<(string token, bool retry)> Authenticate(
            HttpRequestMessage msg, bool retry,
            CancellationToken cancellationToken = default)
        {
            string token;
            // Due to all the additional params appSignAuth is requiring,
            // it's makes sense to still keep it in Http to manage all the details.
            // TODO: Refactoring as part of DEVEXP-1187, ApplicationSignedAuth breaks the ISinchAuth contract
            if (_auth.Value is ApplicationSignedAuth appSignAuth)
            {
                var now = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
                const string headerName = "x-timestamp";
                msg.Headers.Add(headerName, now);

                var bytes = Array.Empty<byte>();
                if (msg.Content is not null)
                {
                    bytes = await msg.Content.ReadAsByteArrayAsync(cancellationToken);
                }

                if (msg.RequestUri is null)
                {
                    throw new NullReferenceException("HttpRequestMessage request uri is null");
                }

                token = appSignAuth.GetSignedAuth(
                    bytes,
                    msg.Method.ToString().ToUpperInvariant(), msg.RequestUri!.PathAndQuery,
                    $"{headerName}:{now}", msg.Content?.Headers.ContentType?.ToString());
                retry = false;
            }
            else
            {
                // try force get new token if retrying
                token = await _auth.Value.GetAuthToken(force: !retry);
            }

            return (token, retry);
        }

        public async Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest? request,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null)
        {
            HttpContent? httpContent =
                request == null ? null : JsonContent.Create(request, options: _jsonSerializerOptions);


            return await SendHttpContent<TResponse>(uri: uri, httpMethod: httpMethod, httpContent,
                cancellationToken: cancellationToken, headers: headers);
        }

        private static string BuildUserAgent()
        {
            var sdkVersion = new AssemblyName(typeof(Http).GetTypeInfo().Assembly.FullName!).Version!.ToString(3);
            var frameworkDescription = RuntimeInformation.FrameworkDescription;
            var runtimeIdentifier = RuntimeInformation.RuntimeIdentifier;

            return $"sinch-sdk/{sdkVersion} (csharp/{frameworkDescription}; {runtimeIdentifier};)";
        }
    }
}
