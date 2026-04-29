using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Logger;

namespace Sinch.Voice.SinchEvents
{
    /// <inheritdoc />
    internal sealed class VoiceSinchEvents(
        JsonSerializerOptions jsonSerializerOptions,
        ILoggerAdapter<IVoiceSinchEvents>? logger = null)
        : IVoiceSinchEvents
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = jsonSerializerOptions;

        /// <inheritdoc />
        public IVoiceSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<IVoiceSinchEvent>(json, JsonSerializerOptions);
            if (result == null)
            {
                logger?.LogError(
                    "Failed to deserialize Voice Sinch event. No matching event type found for payload: {json}", json);
                throw new InvalidOperationException("Deserialization of Voice Sinch event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public IVoiceSinchEvent ParseEvent(JsonNode json)
        {
            var result = json.Deserialize<IVoiceSinchEvent>(JsonSerializerOptions);
            if (result == null)
            {
                logger?.LogError(
                    "Failed to deserialize Voice Sinch event. No matching event type found for payload: {json}",
                    json.ToJsonString());
                throw new InvalidOperationException("Deserialization of Voice Sinch event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<IVoiceSinchEvent> ParseEventAsync(Stream jsonStream,
            CancellationToken cancellationToken = default)
        {
            var result =
                await JsonSerializer.DeserializeAsync<IVoiceSinchEvent>(jsonStream, JsonSerializerOptions,
                    cancellationToken);
            if (result == null)
            {
                logger?.LogError("Failed to deserialize Voice Sinch event. No matching event type found.");
                throw new InvalidOperationException("Deserialization of Voice Sinch event failed");
            }

            return result;
        }
    }
}
