using System.Collections.Generic;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Messages.List
{
    public class ListMessagesResponse
    {
        /// <summary>
        ///     List of messages associated to the referenced conversation.
        /// </summary>
        public IEnumerable<ConversationMessage> Messages { get; set; }

        /// <summary>
        ///     Token that should be included in the next request to fetch the next page.
        /// </summary>
        public string NextPageToken { get; set; }
    }
}
