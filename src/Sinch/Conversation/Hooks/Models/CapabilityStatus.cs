using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    /// Status indicating the recipient&#39;s capability on the channel.
    /// </summary>
    /// <value>Status indicating the recipient&#39;s capability on the channel.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<CapabilityStatus>))]
    public record CapabilityStatus(string Value) : EnumRecord(Value)
    {

        public static readonly CapabilityStatus CapabilityUnknown = new("CAPABILITY_UNKNOWN");
        public static readonly CapabilityStatus CapabilityFull = new("CAPABILITY_FULL");
        public static readonly CapabilityStatus CapabilityPartial = new("CAPABILITY_PARTIAL");
        public static readonly CapabilityStatus NoCapability = new("NO_CAPABILITY");
    }
}
