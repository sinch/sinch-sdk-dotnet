using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Voice.Hooks
{
    [JsonConverter(typeof(EnumRecordJsonConverter<EventType>))]
    public record EventType(string Value) : EnumRecord(Value)
    {
        public static readonly EventType AnsweredCallEvent = new EventType("ace");
        public static readonly EventType DisconnectedCallEvent = new EventType("dice");
        public static readonly EventType IncomingCallEvent = new EventType("ice");
        public static readonly EventType NotificationEvent = new EventType("notify");
        public static readonly EventType PromptInputEvent = new EventType("pie");
    }
}
