namespace Sinch.Conversation.Conversations.List
{
    public sealed class ListRecentConversationsRequest
    {
        /// <summary>
        ///     Required. The application ID.
        /// </summary>
        public required string AppId { get; set; }

        /// <summary>
        ///     True if only active conversations should be listed. Default is false.
        /// </summary>
        public bool? OnlyActive { get; set; }

        /// <summary>
        ///     The maximum number of conversations to fetch. Defaults to 10 and the maximum value is 50.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Next page token previously returned if any.
        /// </summary>
        public string? PageToken { get; set; }

        /// <summary>
        ///     Whether to sort conversations by newest message first or oldest. Default is DESC (newest first).
        /// </summary>
        public ConversationOrder? Order { get; set; }
    }
}
