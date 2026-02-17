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
        /// <param name="headers"></param>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default, Dictionary<string, IEnumerable<string>>? headers = null);

        Task<TResponse> SendMultipart<TRequest, TResponse>(Uri uri, TRequest request, Stream stream, string fileName,
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
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<TResponse> SendMultipart<TRequest, TResponse>(Uri uri, TRequest request, Stream stream,
            string fileName, CancellationToken cancellationToken = default)
        {
            var boundary = Guid.NewGuid().ToString();
            var multipartContent = BuildMultipartFormDataBody<TRequest>(request, stream, fileName, boundary);
            
            var content = new ByteArrayContent(multipartContent);
            content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data")
            {
                Parameters = { new NameValueHeaderValue("boundary", boundary) }
            };

            return await SendHttpContent<TResponse>(uri, HttpMethod.Post, content, cancellationToken);
        }

        /// <summary>
        ///     Manually builds properly formatted multipart/form-data body.
        ///     Uses quoted field names per RFC 7578 to match curl and form-data npm library behavior.
        /// </summary>
        private static byte[] BuildMultipartFormDataBody<TRequest>(TRequest request, Stream fileStream, string fileName, string boundary)
        {
            var body = new MemoryStream();
            var writer = new StreamWriter(body, System.Text.Encoding.UTF8, leaveOpen: true);

            // Local helper functions
            bool DoesntHaveJsonIgnoreAttribute(PropertyInfo prop)
            {
                return !prop.GetCustomAttributes(typeof(JsonIgnoreAttribute)).Any();
            }

            bool HasNonNullValue(PropertyInfo x)
            {
                return x.GetValue(request) != null;
            }

            string ToCamelCase(string pascalCaseName)
            {
                if (string.IsNullOrEmpty(pascalCaseName) || char.IsLower(pascalCaseName[0]))
                {
                    return pascalCaseName;
                }
                return char.ToLowerInvariant(pascalCaseName[0]) + pascalCaseName.Substring(1);
            }

            void WriteFormField(string fieldName, string fieldValue)
            {
                writer.Write($"--{boundary}\r\n");
                writer.Write($"Content-Disposition: form-data; name=\"{fieldName}\"\r\n");
                writer.Write("\r\n");
                writer.Write(fieldValue);
                writer.Write("\r\n");
            }

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
                var fieldName = ToCamelCase(prop.Name);

                if (type == typeof(List<string>))
                {
                    var asStringList = value as List<string>;
                    foreach (var item in asStringList!)
                    {
                        WriteFormField(fieldName, item);
                    }
                }
                else if (type == typeof(Dictionary<string, string>))
                {
                    foreach (var (key, val) in (value as Dictionary<string, string>)!)
                    {
                        var strVal = fieldName + "[" + key + "]";
                        WriteFormField(strVal, val);
                    }
                }
                else
                {
                    var str = value.ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        WriteFormField(fieldName, str);
                    }
                }
            }

            // Add file field only if stream has content
            if (fileStream != null)
            {
                fileStream.Position = 0;
                if (fileStream.Length > 0)
                {
                    writer.Write($"--{boundary}\r\n");
                    writer.Write($"Content-Disposition: form-data; name=\"file\"; filename=\"{fileName}\"\r\n");
                    writer.Write("Content-Type: application/octet-stream\r\n");
                    writer.Write("\r\n");
                    writer.Flush();

                    fileStream.CopyTo(body);
                    writer.Write("\r\n");
                }
            }

            // Final boundary
            writer.Write($"--{boundary}--\r\n");
            writer.Flush();

            var result = body.ToArray();
            writer.Dispose();
            body.Dispose();
            return result;
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
                if (httpContent is MultipartFormDataContent mfd)
                {
                    Debug.WriteLine($"[MULTIPART] Content-Type: {httpContent.Headers.ContentType}");
                    // For multipart, we can't easily read the body without consuming it
                    Debug.WriteLine($"[MULTIPART] Multipart form data request");
                }
                else
                {
                    Debug.WriteLine($"Request body: {httpContent?.ReadAsStringAsync(cancellationToken).Result}");
                }
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
