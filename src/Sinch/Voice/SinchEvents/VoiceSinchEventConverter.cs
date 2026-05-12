using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     JSON converter for <see cref="VoiceSinchEvent" /> that uses the <c>"event"</c> discriminator field
    ///     to determine the concrete event type.
    /// </summary>
    public sealed class VoiceSinchEventConverter : JsonConverter<VoiceSinchEvent>
    {
        /// <inheritdoc />
        public override VoiceSinchEvent? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "event");
            var type = descriptor.Value.GetString();

            if (type == EventType.NotificationEvent.Value)
            {
                return elem.Deserialize<NotificationEvent>(options);
            }

            if (type == EventType.IncomingCallEvent.Value)
            {
                return elem.Deserialize<IncomingCallEvent>(options);
            }

            if (type == EventType.DisconnectedCallEvent.Value)
            {
                return elem.Deserialize<DisconnectedCallEvent>(options);
            }

            if (type == EventType.AnsweredCallEvent.Value)
            {
                return elem.Deserialize<AnsweredCallEvent>(options);
            }

            if (type == EventType.PromptInputEvent.Value)
            {
                return elem.Deserialize<PromptInputEvent>(options);
            }

            throw new JsonException($"Failed to match Voice Sinch Event type, got {descriptor.Value.GetString()}");
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, VoiceSinchEvent value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case AnsweredCallEvent answeredCallEvent:
                    JsonSerializer.Serialize(writer, answeredCallEvent, options);
                    break;
                case DisconnectedCallEvent disconnectedCallEvent:
                    JsonSerializer.Serialize(writer, disconnectedCallEvent, options);
                    break;
                case IncomingCallEvent incomingCallEvent:
                    JsonSerializer.Serialize(writer, incomingCallEvent, options);
                    break;
                case NotificationEvent notificationEvent:
                    JsonSerializer.Serialize(writer, notificationEvent, options);
                    break;
                case PromptInputEvent promptInputEvent:
                    JsonSerializer.Serialize(writer, promptInputEvent, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a matching class for {nameof(VoiceSinchEvent)}");
            }
        }
    }
}
