using System.Collections.Generic;
using Sinch.Core;

namespace Sinch.Conversation.Messages
{
    public class Identified : OneOf<Contact, Identified>
    {
        public IdentifiedBy IdentifiedBy { get; set; }
    }

    public class IdentifiedBy
    {
        public List<ChannelIdentity> ChannelIdentities { get; set; }
    }

    public class ChannelIdentity
    {
        public string Identity { get; set; }

        public ConversationChannel Channel { get; set; }
    }
}
