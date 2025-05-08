namespace Sinch.Conversation.Events.ContactEvents
{
    /// <summary>
    ///     The user sent a comment outside of the main conversation context
    /// </summary>
    public sealed class CommentEvent
    {
        /// <summary>
        ///     Required. The text of the comment.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Text { get; set; }
#else
        public string Text { get; set; } = null!;
#endif
        /// <summary>
        ///     Optional. An identifier for the comment.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     Optional. The type of the comment.
        /// </summary>
        public string? CommentType { get; set; }

        /// <summary>
        ///     Optional. The origin of the comment
        /// </summary>
        public string? CommentedOn { get; set; }

        /// <summary>
        ///     Optional. The user that triggered the comment event
        /// </summary>
        public string? User { get; set; }
    }
}
