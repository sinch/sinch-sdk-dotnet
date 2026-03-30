using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Common
{
    public sealed class Identified : IRecipient
    {
        public IdentifiedBy? IdentifiedBy { get; set; }
    }

    public sealed class IdentifiedBy
    {
        [JsonPropertyName("channel_identities")]
        public List<ChannelRecipientIdentity>? ChannelIdentities { get; set; }
    }
}
