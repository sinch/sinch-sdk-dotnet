using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     Marker interface for event types of voice.
    /// </summary>
    [JsonConverter(typeof(VoiceSinchEventConverter))]
    public abstract class VoiceSinchEvent
    {
        [JsonPropertyName("event")]
        [JsonInclude]
        internal abstract EventType Event { get; set; }
    }

    public sealed class VoiceSinchEventConverter : JsonConverter<VoiceSinchEvent>
    {
        public override VoiceSinchEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "event");
            var type = descriptor.Value.GetString();

            if (type == EventType.NotificationEvent.Value)
            {
                return elem.Deserialize<NotificationSinchEvent>(options);
            }

            if (type == EventType.IncomingCallEvent.Value)
            {
                return elem.Deserialize<IncomingCallSinchEvent>(options);
            }

            if (type == EventType.DisconnectedCallEvent.Value)
            {
                return elem.Deserialize<DisconnectedCallSinchEvent>(options);
            }

            if (type == EventType.AnsweredCallEvent.Value)
            {
                return elem.Deserialize<AnsweredCallSinchEvent>(options);
            }

            if (type == EventType.PromptInputEvent.Value)
            {
                return elem.Deserialize<PromptInputSinchEvent>(options);
            }

            throw new JsonException($"Failed to match verification method object, got {descriptor.Name}");
        }

        public override void Write(Utf8JsonWriter writer, VoiceSinchEvent value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case AnsweredCallSinchEvent answeredCallEvent:
                    JsonSerializer.Serialize(writer, answeredCallEvent, options);
                    break;
                case DisconnectedCallSinchEvent disconnectedCallEvent:
                    JsonSerializer.Serialize(writer, disconnectedCallEvent, options);
                    break;
                case IncomingCallSinchEvent incomingCallEvent:
                    JsonSerializer.Serialize(writer, incomingCallEvent, options);
                    break;
                case NotificationSinchEvent notificationEvent:
                    JsonSerializer.Serialize(writer, notificationEvent, options);
                    break;
                case PromptInputSinchEvent promptInputEvent:
                    JsonSerializer.Serialize(writer, promptInputEvent, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a matching class for the interface {nameof(VoiceSinchEvent)}");
            }
        }
    }
}
