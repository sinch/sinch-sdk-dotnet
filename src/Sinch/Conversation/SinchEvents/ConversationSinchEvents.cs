using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Logger;

namespace Sinch.Conversation.SinchEvents
{
    /// <inheritdoc />
    internal sealed class ConversationSinchEvents(
        JsonSerializerOptions jsonSerializerOptions,
        ILoggerAdapter<IConversationSinchEvents>? logger = null)
        : IConversationSinchEvents
    {
        /// <inheritdoc />
        public JsonSerializerOptions JsonSerializerOptions { get; } = jsonSerializerOptions;

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(IDictionary<string, string> headers, string body, string secret)
        {
            return HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, body);
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(IReadOnlyDictionary<string, IEnumerable<string>> headers, string body,
            string secret)
        {
            return HmacAuthenticationValidation.ValidateAuthenticationHeader(secret, headers, body);
        }

        /// <inheritdoc />
        public IConversationSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<IConversationSinchEvent>(json, JsonSerializerOptions);
            if (result == null)
            {
                logger?.LogError(
                    "Failed to deserialize Conversation Sinch event. No matching event type found for payload: {json}",
                    json);
                throw new InvalidOperationException("Deserialization of Conversation Sinch event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<IConversationSinchEvent> ParseEventAsync(Stream jsonStream,
            CancellationToken cancellationToken = default)
        {
            var result =
                await JsonSerializer.DeserializeAsync<IConversationSinchEvent>(jsonStream, JsonSerializerOptions,
                    cancellationToken);
            if (result == null)
            {
                logger?.LogError("Failed to deserialize Conversation Sinch event. No matching event type found.");
                throw new InvalidOperationException("Deserialization of Conversation Sinch event failed");
            }

            return result;
        }
    }
}
