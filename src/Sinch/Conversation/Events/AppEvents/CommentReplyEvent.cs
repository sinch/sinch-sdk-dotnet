namespace Sinch.Conversation.Events.AppEvents
{
    public sealed class CommentReplyEvent
    {
        /// <summary>
        ///     The text of the comment reply.
        /// </summary>
        public string? Text { get; set; }
    }
}
