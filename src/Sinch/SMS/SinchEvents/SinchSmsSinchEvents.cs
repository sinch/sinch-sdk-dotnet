using System;
using System.Collections.Generic;
using System.Text.Json;
using Sinch.Logger;

namespace Sinch.SMS.SinchEvents
{
    /// <inheritdoc />
    internal sealed class SinchSmsSinchEvents(
        JsonSerializerOptions jsonSerializerOptions,
        ILoggerAdapter<ISinchSmsSinchEvents>? logger = null)
        : ISinchSmsSinchEvents
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = jsonSerializerOptions;

        /// <inheritdoc />
        public ISmsEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<ISmsEvent>(json, JsonSerializerOptions);
            if (result == null)
            {
                logger?.LogError("Failed to deserialize SMS Sinch event. No matching event type found for payload: {json}", json);
                throw new InvalidOperationException("Deserialization of SMS Sinch event failed");
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
