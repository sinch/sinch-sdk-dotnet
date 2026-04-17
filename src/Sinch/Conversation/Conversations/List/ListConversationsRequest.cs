namespace Sinch.Conversation.Conversations.List
{
    public sealed class ListConversationsRequest
    {
        /// <summary>
        ///     Set to <c>true</c> to list only active conversations. Set to <c>false</c> to list only inactive conversations.
        /// </summary>
        public bool? OnlyActive { get; set; }


        /// <summary>
        ///     At least one of app_id or contact_id must be present.
        /// </summary>
        public string? AppId { get; set; }

        /// <summary>
        ///     At least one of app_id or contact_id must be present.
        /// </summary>
        public string? ContactId { get; set; }

        /// <summary>
        ///     The maximum number of conversations to fetch. Defaults to 10 and the maximum is 20.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Next page token previously returned if any.
        /// </summary>
        public string? PageToken { get; set; }

        /// <summary>
        ///     Only fetch conversations from the active_channel
        /// </summary>
        public ConversationChannel? ActiveChannel { get; set; }
    }
}
