using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Logger;

namespace Sinch.SMS.SinchEvents
{
    /// <inheritdoc />
    internal sealed class SmsSinchEvents(
        JsonSerializerOptions jsonSerializerOptions,
        ILoggerAdapter<ISmsSinchEvents>? logger = null)
        : ISmsSinchEvents
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = jsonSerializerOptions;

        /// <inheritdoc />
        public ISmsSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<ISmsSinchEvent>(json, JsonSerializerOptions);
            if (result == null)
            {
                logger?.LogError("Failed to deserialize SMS Sinch event. No matching event type found for payload: {json}", json);
                throw new InvalidOperationException("Deserialization of SMS Sinch event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<ISmsSinchEvent> ParseEventAsync(Stream json, CancellationToken cancellationToken = default)
        {
            var result = await JsonSerializer.DeserializeAsync<ISmsSinchEvent>(json, JsonSerializerOptions, cancellationToken);
            if (result == null)
            {
                logger?.LogError("Failed to deserialize SMS Sinch event. No matching event type found.");
                throw new InvalidOperationException("Deserialization of SMS Sinch event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(
            string secret,
            IDictionary<string, IEnumerable<string>> headers,
            string body)
        {
            return HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, body);
        }
    }
}
