using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Numbers.SinchEvents
{
    internal sealed class NumbersSinchEvents : INumbersSinchEvents
    {
        private readonly ILoggerAdapter<INumbersSinchEvents>? _logger;

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
            IDictionary<string, IEnumerable<string>> headers,
            string body)
        {
            if (!headers.TryGetValue("x-sinch-signature", out var values))
                return false;
            var signature = values?.FirstOrDefault();
            if (string.IsNullOrEmpty(signature))
                return false;
            return HeaderValidation.ValidateAuthHeader(hmacSecret, body, signature);
        }
    }
}
