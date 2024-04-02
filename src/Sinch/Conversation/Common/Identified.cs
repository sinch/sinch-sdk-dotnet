using System.Collections.Generic;
using Sinch.Conversation.Messages;

namespace Sinch.Conversation.Common
{
    public class Identified : IRecipient
    {
        public IdentifiedBy IdentifiedBy { get; set; }
    }

    public class IdentifiedBy
    {
        public List<ChannelIdentity> ChannelIdentities { get; set; }
    }
}
