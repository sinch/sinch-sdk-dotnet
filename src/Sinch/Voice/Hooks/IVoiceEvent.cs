using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Hooks
{
    /// <summary>
    ///     Marker interface for event types of voice.
    /// </summary>
    [JsonConverter(typeof(InterfaceConverter<IVoiceEvent>))]
    public interface IVoiceEvent
    {
        [JsonPropertyName("event")]
        public EventType? Event { get; }
    }

    public class VoiceEventConverter : JsonConverter<IVoiceEvent>
    {
        public override IVoiceEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            throw new JsonException($"Failed to match verification method object, got {descriptor.Name}");
        }

        public override void Write(Utf8JsonWriter writer, IVoiceEvent value, JsonSerializerOptions options)
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
                        $"Cannot find a matching class for the interface {nameof(IVoiceEvent)}");
            }
        }
    }
}
