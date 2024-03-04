using Sinch.Conversation.Events.AppEvents;

namespace Sinch.Conversation.Common
{
    /// <summary>
    ///     Represents an agent that is involved in a conversation.
    /// </summary>
    public class Agent
    {
        /// <summary>
        ///     Agent's display name
        /// </summary>
        public string DisplayName { get; set; }

        public AgentType Type { get; set; }

        /// <summary>
        ///     The Agent's picture url.
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
