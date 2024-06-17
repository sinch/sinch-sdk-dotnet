using System;
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
            CancellationToken cancellationToken = default, HttpHeaders? headers = null);
        
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
            CancellationToken cancellationToken = default, HttpHeaders? headers = null);
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
        
        public Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default, HttpHeaders? headers = null)
        {
            return Send<object, TResponse>(uri, httpMethod, null, cancellationToken, headers);
        }
        
        public async Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest request,
            CancellationToken cancellationToken = default, HttpHeaders? headers = null)
        {
            var retry = true;
            while (true)
            {
                _logger?.LogDebug("Sending request to {uri}", uri);
                HttpContent? httpContent =
                    request == null ? null : JsonContent.Create(request, options: _jsonSerializerOptions);
                
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
                
                await result.EnsureSuccessApiStatusCode();
                _logger?.LogDebug("Finished processing request for {uri}", uri);
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
    }
}
