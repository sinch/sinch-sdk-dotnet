using System.Collections.Generic;

namespace Sinch.Conversation.Conversations.List
{
    public sealed class ListConversationsResponse
    {
        /// <summary>
        ///     List of conversations matching the search query.
        /// </summary>
        public IEnumerable<Conversation> Conversations { get; set; }

        /// <summary>
        ///     Token that should be included in the next request to fetch the next page.
        /// </summary>
        public string NextPageToken { get; set; }

        /// <summary>
        ///     Total count of conversations
        /// </summary>
        public int TotalSize { get; set; }
    }
}
