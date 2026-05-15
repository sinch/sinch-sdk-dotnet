using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using Sinch.Logger;
using Sinch.Numbers.SinchEvents;

namespace Sinch.Fax.SinchEvents
{
    internal sealed class FaxSinchEvents : IFaxSinchEvents
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILoggerAdapter<IFaxSinchEvents>? _logger;

        private const string SinchSignature = "x-sinch-signature";

        internal FaxSinchEvents(
            JsonSerializerOptions jsonSerializerOptions,
            ILoggerAdapter<IFaxSinchEvents>? logger = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
            _logger = logger;
        }

        /// <inheritdoc />
        public IFaxSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<IFaxSinchEvent>(json, _jsonSerializerOptions);
            if (result is null)
            {
                _logger?.LogError("Failed to deserialize Fax Sinch Event. Payload: {json}", json);
                throw new InvalidOperationException("Deserialization of Fax Sinch Event failed.");
            }

            return result;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
