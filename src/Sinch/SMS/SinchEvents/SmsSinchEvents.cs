using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using Sinch.Logger;

namespace Sinch.SMS.SinchEvents
{
    /// <inheritdoc />
    internal sealed class SmsSinchEvents : ISmsSinchEvents
    {
        private readonly ILoggerAdapter<ISmsSinchEvents>? _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        internal SmsSinchEvents(
            JsonSerializerOptions jsonSerializerOptions,
            ILoggerAdapter<ISmsSinchEvents>? logger = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
            _logger = logger;
        }

        /// <inheritdoc />
        public ISmsSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<ISmsEvent>(json, _jsonSerializerOptions) as ISmsSinchEvent;
            if (result is null)
            {
                _logger?.LogError("Failed to deserialize SMS Sinch Event. No matching event type found for payload: {json}", json);
                throw new InvalidOperationException("Deserialization of SMS Sinch Event failed.");
            }

            return result;
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(
            string hmacSecret,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
            string body)
        {
            return ValidateSignature(hmacSecret, headers, body);
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

        private static bool ValidateSignature(
            string hmacSecret,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
            string body)
        {
            var headerDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var header in headers)
            {
                var value = header.Value?.FirstOrDefault();
                if (value != null)
                {
                    headerDict[header.Key] = value;
                }
            }

            return HmacAuthenticationValidation.ValidateAuthenticationHeader(hmacSecret, headerDict, body);
        }
    }
}
