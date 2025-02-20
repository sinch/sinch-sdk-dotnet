using Sinch.Conversation.Common;

namespace Sinch.Conversation.Events.AppEvents
{
    public sealed class AgentJoinedEvent
    {
        public Agent? Agent { get; set; }
    }
}
