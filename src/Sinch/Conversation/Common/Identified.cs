using System.Collections.Generic;

namespace Sinch.Conversation.Common
{
    public sealed class Identified : IRecipient
    {
        public IdentifiedBy? IdentifiedBy { get; set; }
    }

    public sealed class IdentifiedBy
    {
        public List<ChannelIdentity>? ChannelIdentities { get; set; }
    }
}
