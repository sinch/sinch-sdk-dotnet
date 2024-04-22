using System.Collections.Generic;

namespace Sinch.Conversation.Common
{
    public class Identified : IRecipient
    {
        public IdentifiedBy? IdentifiedBy { get; set; }
    }

    public class IdentifiedBy
    {
        public List<ChannelIdentity>? ChannelIdentities { get; set; }
    }
}
