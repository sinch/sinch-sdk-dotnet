using Sinch.Conversation.Messages;

namespace Sinch.Conversation.Conversations.List
{
    public sealed class ListConversationsRequest
    {
        /// <summary>
        ///     Required. True if only active conversations should be listed.
        /// </summary>
#if NET7_0_OR_GREATER
        public required bool OnlyActive { get; set; }
#else
        public bool OnlyActive { get; set; }
#endif

        /// <summary>
        ///     At least one of app_id or contact_id must be present.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        ///     At least one of app_id or contact_id must be present.
        /// </summary>
        public string ContactId { get; set; }

        /// <summary>
        ///     The maximum number of conversations to fetch. Defaults to 10 and the maximum is 20.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Next page token previously returned if any.
        /// </summary>
        public string PageToken { get; set; }

        /// <summary>
        ///     Only fetch conversations from the active_channel
        /// </summary>
        public ConversationChannel ActiveChannel { get; set; }
    }
}
