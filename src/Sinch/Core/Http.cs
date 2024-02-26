﻿using System;
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
        public Task<TResponse> SendMultipart<TRequest, TResponse>(Uri uri, TRequest request, Stream stream, string fileName, CancellationToken cancellationToken = default);
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
        private readonly ISinchAuth _auth;
        private readonly string _userAgentHeaderValue;


        public Http(ISinchAuth auth, HttpClient httpClient, ILoggerAdapter<Http> logger,
            JsonNamingPolicy jsonNamingPolicy)
        {
            _logger = logger;
            _auth = auth;
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = jsonNamingPolicy,
                AllowTrailingCommas = true,
                
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters ={
                    new JsonStringEnumConverter()
                }
            };
            var sdkVersion = new AssemblyName(typeof(Http).GetTypeInfo()!.Assembly!.FullName!).Version!.ToString();
            _userAgentHeaderValue =
                $"sinch-sdk/{sdkVersion} (csharp/{RuntimeInformation.FrameworkDescription};;)";
        }
        public Task<TResponse> SendMultipart<TRequest, TResponse>(Uri uri, TRequest request, Stream stream, string fileName, CancellationToken cancellationToken = default)
        {

            var content = new MultipartFormDataContent();
            var props = request.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.DeclaredOnly);
            foreach (var prop in props)
            {
                var value = prop.GetValue(request);
                if (value != null)
                {
                    content.Add(new StringContent(value.ToString()), prop.Name);
                }
            }
            stream.Position = 0;
            content.Add(new StreamContent(stream), "file", fileName);
            return SendHttpContent<TResponse>(uri, HttpMethod.Post, content, cancellationToken);

        }
        public Task<TResponse> Send<TResponse>(Uri uri, HttpMethod httpMethod,
            CancellationToken cancellationToken = default)
        {
            return Send<object, TResponse>(uri, httpMethod, null, cancellationToken);
        }


        private async Task<TResponse> SendHttpContent<TResponse>(Uri uri, HttpMethod httpMethod, HttpContent httpContent,
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
                    token = appSignAuth.GetSignedAuth(
                        msg.Content?.ReadAsByteArrayAsync(cancellationToken).GetAwaiter().GetResult(),
                        msg.Method.ToString().ToUpperInvariant(), msg.RequestUri.PathAndQuery,
                        $"{headerName}:{now}", msg.Content?.Headers?.ContentType?.ToString());
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
                    if (_auth.Scheme == AuthSchemes.Bearer && true == result.Headers?.Contains(wwwAuthenticateHeader) &&
                        false == result.Headers?.GetValues(wwwAuthenticateHeader)?.Contains("expired"))
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
                foreach (var header in result.Headers)
                {
                    _logger?.LogDebug("Header: {header} = {value}", header.Key, header.Value);
                }
                
                var json = await result.Content.ReadAsStringAsync(cancellationToken);
                
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

        public async Task<TResponse> Send<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest request,
                CancellationToken cancellationToken = default)
        {

            HttpContent httpContent =
                request == null ? null : JsonContent.Create(request, options: _jsonSerializerOptions);


            return await SendHttpContent<TResponse>(uri: uri, httpMethod: httpMethod, httpContent: httpContent, cancellationToken: cancellationToken);
        }

        public Task<TResponse> SendMultipart<TRequest, TResponse>(Uri uri, HttpMethod httpMethod, TRequest request, Stream stream, string fileName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

