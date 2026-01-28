using System;
using System.Collections.Generic;
using System.Text.Json;
using Sinch.Logger;

namespace Sinch.SMS.Hooks
{
    /// <inheritdoc />
    internal sealed class SmsWebhooks : ISmsWebhooks
    {
        private readonly HmacAuthenticationValidation _authenticationChecker;
        private readonly ILoggerAdapter<ISmsWebhooks>? _logger;

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public SmsWebhooks(JsonSerializerOptions jsonSerializerOptions,
            ILoggerAdapter<ISmsWebhooks>? logger = null)
        {
            JsonSerializerOptions = jsonSerializerOptions;
            _logger = logger;
        }

        /// <inheritdoc />
        public ISmsEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<ISmsEvent>(json, JsonSerializerOptions);
            if (result == null)
            {
                _logger?.LogError("Failed to deserialize SMS webhook event. No matching event type found for payload: {json}", json);
                throw new InvalidOperationException("Deserialization of SMS webhook event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(
            string secret,
            IDictionary<string, string> headers,
            string body)
        {
            return HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, body);
        }
    }
}
