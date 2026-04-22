using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.EventDestinations
{
    /// <summary>
    ///     Defines EventDestinationTargetType
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<EventDestinationTargetType>))]
    public record EventDestinationTargetType(string Value) : EnumRecord(Value)
    {
        public static readonly EventDestinationTargetType Dismiss = new("DISMISS");
        public static readonly EventDestinationTargetType Http = new("HTTP");
    }
}
