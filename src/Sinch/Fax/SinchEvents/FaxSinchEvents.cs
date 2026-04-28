using System;
using System.Collections.Generic;
using System.Text.Json;
using Sinch.Logger;

namespace Sinch.Fax.SinchEvents
{
    /// <inheritdoc />
    internal sealed class FaxSinchEvents(
        JsonSerializerOptions jsonSerializerOptions,
        ILoggerAdapter<ISinchFaxSinchEvents>? logger = null)
        : ISinchFaxSinchEvents
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
        public bool ValidateAuthenticationHeader(IDictionary<string, string> headers, string body)
        {
            // No header validation is defined for the Fax API.
            return true;
        }
    }
}
