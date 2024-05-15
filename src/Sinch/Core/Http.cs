using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Auth;
using Sinch.Fax.Faxes;
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
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default);

        Task<TResponse> SendMultipart<TRequest, TResponse>(Uri uri, TRequest request, Stream stream, string fileName,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Use to send http request with a body
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="httpContent"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TRequest">The type of the request object.</typeparam>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest httpContent,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    ///     Represents an empty response for cases where no json is expected.
    /// </summary>
    public class EmptyResponse
    {
    }

    /// <inheritdoc /> 
    internal class Http : IHttp
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILoggerAdapter<IHttp>? _logger;
        private readonly ISinchAuth _auth;
        private readonly string _userAgentHeaderValue;


        public Http(ISinchAuth auth, HttpClient httpClient, ILoggerAdapter<IHttp>? logger,
            JsonNamingPolicy jsonNamingPolicy)
        {
            _logger = logger;
            _auth = auth;
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = jsonNamingPolicy,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var sdkVersion = new AssemblyName(typeof(Http).GetTypeInfo().Assembly.FullName!).Version!.ToString();
            _userAgentHeaderValue =
                $"sinch-sdk/{sdkVersion} (csharp/{RuntimeInformation.FrameworkDescription};;)";
        }

        public Task<TResponse> SendMultipart<TRequest, TResponse>(Uri uri, TRequest request, Stream stream,
            string fileName, CancellationToken cancellationToken = default)
        {
            var content = new MultipartFormDataContent();
            var props = request!.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                         BindingFlags.DeclaredOnly)
                .Where(DoesntHaveJsonIgnoreAttribute).Where(HasNonNullValue);
            foreach (var prop in props)
            {
                var value = prop.GetValue(request);
                if (value == null)
                {
                    continue;
                }

                var type = value.GetType();
                if (type == typeof(List<string>))
                {
                    var asString = string.Join(',', (value as List<string>)!);
                    content.Add(new StringContent(asString), prop.Name);
                }
                else if (type == typeof(Dictionary<string, string>))
                {
                    foreach (var (key, val) in (value as Dictionary<string, string>)!)
                    {
                        var strVal = prop.Name + "[" + key + "]";
                        content.Add(new StringContent(val), strVal);
                    }
                }
                else
                {
                    var str = value.ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        content.Add(new StringContent(str), prop.Name);
                    }
                }
            }

            stream.Position = 0;
            var isContentType = new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);
            var streamContent = new StreamContent(stream)
            {
                Headers =
                {
                    ContentType = isContentType ? new MediaTypeHeaderValue(contentType!) : null
                }
            };
            content.Add(streamContent, "file", fileName);


            return SendHttpContent<TResponse>(uri, HttpMethod.Post, content, cancellationToken);

            bool DoesntHaveJsonIgnoreAttribute(PropertyInfo prop)
            {
                return !prop.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
            }

            bool HasNonNullValue(PropertyInfo x)
            {
                return x.GetValue(request) != null;
            }
        }

        public Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default)
        {
            return Send<EmptyResponse, TResponse>(uri, httpMethod, null, cancellationToken);
        }

        private async Task<TResponse> SendHttpContent<TResponse>(Uri uri, HttpMethod httpMethod,
            HttpContent? httpContent,
            CancellationToken cancellationToken = default)
        {
            var retry = true;
            while (true)
            {
                _logger?.LogDebug("Sending request to {uri}", uri);

#if DEBUG
                Debug.WriteLine($"Request uri: {uri}");
                Debug.WriteLine($"Request body: {httpContent?.ReadAsStringAsync(cancellationToken).Result}");
#endif

                using var msg = new HttpRequestMessage();
                msg.RequestUri = uri;
                msg.Method = httpMethod;
                msg.Content = httpContent;

                string token;
                // Due to all the additional params appSignAuth is requiring,
                // it's makes sense to still keep it in Http to manage all the details.
                // TODO: get insight how to refactor this ?!?!?!
                if (_auth is ApplicationSignedAuth appSignAuth)
                {
                    var now = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
                    const string headerName = "x-timestamp";
                    msg.Headers.Add(headerName, now);

                    var bytes = Array.Empty<byte>();
                    if (msg.Content is not null)
                    {
                        bytes = await msg.Content.ReadAsByteArrayAsync(cancellationToken);
                    }

                    token = appSignAuth.GetSignedAuth(
                        bytes,
                        msg.Method.ToString().ToUpperInvariant(), msg.RequestUri.PathAndQuery,
                        $"{headerName}:{now}", msg.Content?.Headers.ContentType?.ToString());
                    retry = false;
                }
                else
                {
                    // try force get new token if retrying
                    token = await _auth.GetAuthToken(force: !retry);
                }

                msg.Headers.Authorization = new AuthenticationHeaderValue(_auth.Scheme, token);

                msg.Headers.Add("User-Agent", _userAgentHeaderValue);

                var result = await _httpClient.SendAsync(msg, cancellationToken);

                if (result.StatusCode == HttpStatusCode.Unauthorized && retry)
                {
                    // will not retry when no "expired" header for a token.
                    const string wwwAuthenticateHeader = "www-authenticate";
                    if (_auth.Scheme == AuthSchemes.Bearer && result.Headers.Contains(wwwAuthenticateHeader) &&
                        !result.Headers.GetValues(wwwAuthenticateHeader).Contains("expired"))
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

                if (result.IsPdf())
                {
                    if (typeof(TResponse) != typeof(Stream))
                    {
                        throw new InvalidOperationException(
                            "Received pdf, but expected response type is not a Stream.");
                    }

                    return (TResponse)(object)await result.Content.ReadAsStreamAsync(cancellationToken);
                }

                if (result.IsJson())
                    return await result.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken,
                               options: _jsonSerializerOptions)
                           ?? throw new InvalidOperationException(
                               $"{typeof(TResponse).Name} is null");

                // if empty response is expected, any non related response is dropped
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

                // unexpected content, log warning and throw exception
                _logger?.LogWarning("Response is not json, but {content}",
                    await result.Content.ReadAsStringAsync(cancellationToken));

                throw new InvalidOperationException("The response is not Json or EmptyResponse");
            }
        }

        public async Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest? request,
            CancellationToken cancellationToken = default)
        {
            HttpContent? httpContent =
                request == null ? null : JsonContent.Create(request, options: _jsonSerializerOptions);


            return await SendHttpContent<TResponse>(uri: uri, httpMethod: httpMethod, httpContent,
                cancellationToken: cancellationToken);
        }

        public Task<TResponse> SendMultipart<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest request,
            Stream stream, string fileName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
