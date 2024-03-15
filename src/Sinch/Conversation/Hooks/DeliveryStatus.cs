using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    /// Shows the status of the message or event delivery
    /// </summary>
    /// <value>Shows the status of the message or event delivery</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<DeliveryStatus>))]
    public record DeliveryStatus(string Value) : EnumRecord(Value)
    {
        public static readonly DeliveryStatus QueuedOnChannel = new("QUEUED_ON_CHANNEL");
        public static readonly DeliveryStatus Delivered = new("DELIVERED");
        public static readonly DeliveryStatus Read = new("READ");
        public static readonly DeliveryStatus Failed = new("FAILED");
        public static readonly DeliveryStatus SwitchingChannel = new("SWITCHING_CHANNEL");
    }
}
