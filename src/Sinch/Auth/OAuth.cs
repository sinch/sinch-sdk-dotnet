﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Auth
{
    internal class OAuth : IAuth
    {
        private readonly HttpClient _httpClient;
        private readonly string _keyId;
        private readonly string _keySecret;
        private readonly ILoggerAdapter<OAuth> _logger;
        private DateTime? _expiresIn;
        private volatile string _token;
        private readonly Uri _baseAddress;

        public OAuth(string keyId, string keySecret, HttpClient httpClient, ILoggerAdapter<OAuth> logger)
        {
            _keyId = keyId;
            _keySecret = keySecret;
            _httpClient = httpClient;
            _logger = logger;
            _baseAddress = new Uri("https://auth.sinch.com");
            Scheme = "Bearer";
        }

        internal OAuth(Uri baseAddress, HttpClient httpClient) : this("", "", httpClient, null)
        {
            _baseAddress = baseAddress;
        }

        public async Task<string> GetToken(bool force = false)
        {
            _logger?.LogInformation("Getting token...");
            if (_token is not null && _expiresIn.HasValue && DateTime.UtcNow < _expiresIn.Value && !force)
            {
                _logger?.LogInformation("Returning cached token which will expire at {expire}", _expiresIn);
                return _token;
            }

            _logger?.LogInformation("Fetching new token");
            var @base = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_keyId}:{_keySecret}"));
            var dict = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            };
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_baseAddress, "oauth2/token"),
                Content = new FormUrlEncodedContent(dict),
                Method = HttpMethod.Post,
                Headers = { Authorization = new AuthenticationHeaderValue("Basic", @base) }
            };
            var result = await _httpClient.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                var authResponse = await result.TryGetJson<AuthApiError>();
                _logger?.LogError("Failed to fetch a token with {status} dew to {reason}", result.StatusCode,
                    result.ReasonPhrase);
                throw new AuthException(result.StatusCode, result.ReasonPhrase, null, authResponse);
            }

            var response = await result.Content.ReadFromJsonAsync<AuthResponse>();

            if (response is null)
            {
                _logger?.LogError("Json response is null");
                throw new NullReferenceException("response is null");
            }

            _token = response.AccessToken;

            // Add some grace period for token update just in case
            var expiresIn = response.ExpiresInSeconds;
            if (expiresIn - 5 > 0) expiresIn -= 5;

            _expiresIn = DateTime.UtcNow.AddSeconds(expiresIn);
            _logger?.LogInformation("Retrieved new token which will expire in {expire}", _expiresIn);
            return _token;
        }

        public string Scheme { get; }

        private class AuthResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            /// <summary>
            ///     In seconds
            /// </summary>
            [JsonPropertyName("expires_in")]
            public int ExpiresInSeconds { get; set; }
        }
    }
}
