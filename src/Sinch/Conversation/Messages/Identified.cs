using System.Collections.Generic;
using Sinch.Core;

namespace Sinch.Conversation.Messages
{
    public class Identified : IRecipient
    {
        public IdentifiedBy IdentifiedBy { get; set; }
    }

    public class IdentifiedBy
    {
        public List<ChannelIdentity> ChannelIdentities { get; set; }
    }

    public class ChannelIdentity
    {
        public string AppId { get; set; }

        public string Identity { get; set; }

        public ConversationChannel Channel { get; set; }
    }
}
