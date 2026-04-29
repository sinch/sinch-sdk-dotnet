using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Logger;

namespace Sinch.Fax.SinchEvents
{
    /// <inheritdoc />
    internal sealed class FaxSinchEvents(
        JsonSerializerOptions jsonSerializerOptions,
        ILoggerAdapter<IFaxSinchEvents>? logger = null)
        : IFaxSinchEvents
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = jsonSerializerOptions;

        /// <inheritdoc />
        public IFaxSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<IFaxSinchEvent>(json, JsonSerializerOptions);
            if (result == null)
            {
                logger?.LogError(
                    "Failed to deserialize Fax Sinch event. No matching event type found for payload: {json}", json);
                throw new InvalidOperationException("Deserialization of Fax Sinch event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<IFaxSinchEvent> ParseEventAsync(Stream json, CancellationToken cancellationToken = default)
        {
            var result = await JsonSerializer.DeserializeAsync<IFaxSinchEvent>(json, JsonSerializerOptions, cancellationToken);
            if (result == null)
            {
                logger?.LogError("Failed to deserialize Fax Sinch event. No matching event type found.");
                throw new InvalidOperationException("Deserialization of Fax Sinch event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(IDictionary<string, IEnumerable<string>> headers, string body)
        {
            // No header validation is defined for the Fax API.
            return true;
        }
    }
}
