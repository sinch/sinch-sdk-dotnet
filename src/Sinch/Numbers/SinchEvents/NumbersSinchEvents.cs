using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Numbers.SinchEvents
{
    internal sealed class NumbersSinchEvents : INumbersSinchEvents
    {
        private readonly ILoggerAdapter<INumbersSinchEvents>? _logger;
        
        private const string SinchSignature = "x-sinch-signature";

        internal NumbersSinchEvents(
            JsonSerializerOptions jsonSerializerOptions,
            ILoggerAdapter<INumbersSinchEvents>? logger = null)
        {
            JsonSerializerOptions = jsonSerializerOptions;
            _logger = logger;
        }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public INumbersSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<NumbersSinchEvent>(json, JsonSerializerOptions);
            if (result is null)
            {
                _logger?.LogError("Failed to deserialize Numbers Sinch Event. Payload: {json}", json);
                throw new InvalidOperationException("Deserialization of Numbers Sinch Event failed.");
            }

            return result;
        }

        public bool ValidateAuthenticationHeader(
            string hmacSecret,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
            string body)
        {
            var signature = headers
                .FirstOrDefault(h => string.Equals(h.Key, SinchSignature, StringComparison.OrdinalIgnoreCase))
                .Value?.FirstOrDefault();
            return ValidateSignature(hmacSecret, signature, body);
        }

        public bool ValidateAuthenticationHeader(
            string hmacSecret,
            IEnumerable<KeyValuePair<string, StringValues>> headers,
            string body) =>
            ValidateAuthenticationHeader(
                hmacSecret,
                headers.Select(h => new KeyValuePair<string, IEnumerable<string>>(h.Key, h.Value)),
                body);

        private static bool ValidateSignature(string hmacSecret, string? signature, string body)
        {
            if (string.IsNullOrEmpty(signature))
                return false;
            return HeaderValidation.ValidateAuthHeader(hmacSecret, body, signature);
        }
    }
}
