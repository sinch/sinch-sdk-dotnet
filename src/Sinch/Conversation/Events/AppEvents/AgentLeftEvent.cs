using Sinch.Conversation.Common;

namespace Sinch.Conversation.Events.AppEvents
{
    public sealed class AgentLeftEvent
    {
        public Agent? Agent { get; set; }
    }
}
