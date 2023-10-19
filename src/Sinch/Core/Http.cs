using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <returns></returns>
        Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
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

    /// <inheritdoc /> 
    internal class Http : IHttp
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILoggerAdapter<Http> _logger;
        private readonly IAuth _auth;

        public Http(IAuth auth, HttpClient httpClient, ILoggerAdapter<Http> logger,
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
        }

        public Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default)
        {
            return Send<object, TResponse>(uri, httpMethod, null, cancellationToken);
        }

        public async Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest request,
            CancellationToken cancellationToken = default)
        {
            var retry = true;
            while (true)
            {
                // try force get new token if retrying
                var token = await _auth.GetAuthValue(!retry);
                _logger?.LogDebug("Sending request to {uri}", uri);
                HttpContent httpContent =
                    request == null ? null : JsonContent.Create(request, options: _jsonSerializerOptions);

#if DEBUG
                Debug.WriteLine($"Request uri: {uri}");
                Debug.WriteLine($"Request body: {httpContent?.ReadAsStringAsync(cancellationToken).Result}");
#endif

                using var msg = new HttpRequestMessage
                {
                    RequestUri = uri,
                    Method = httpMethod,
                    Content = httpContent,
                    Headers = { Authorization = new AuthenticationHeaderValue(_auth.Scheme, token) }
                };

                var result = await _httpClient.SendAsync(msg, cancellationToken);

                if (result.StatusCode == HttpStatusCode.Unauthorized && retry)
                {
                    retry = false;
                    continue;
                }

                await result.EnsureSuccessApiStatusCode();
                _logger?.LogDebug("Finished processing request for {uri}", uri);
                if (result.IsJson())
                    return await result.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken,
                               options: _jsonSerializerOptions)
                           ?? throw new NullReferenceException(
                               $"{nameof(TResponse)} is null");

                _logger?.LogWarning("Response is not json, but {content}",
                    await result.Content.ReadAsStringAsync(cancellationToken));
                return default;
            }
        }
    }
}
