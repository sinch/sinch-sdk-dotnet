using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Conversations.List
{
    /// <summary>
    ///     A conversation and its most recent message.
    /// </summary>
    public sealed class ConversationRecentMessage
    {
        /// <summary>
        ///     The conversation.
        /// </summary>
        public Conversation? Conversation { get; set; }

        /// <summary>
        ///     The most recent message in the conversation.
        /// </summary>
        public ConversationMessage? LastMessage { get; set; }
    }
}
