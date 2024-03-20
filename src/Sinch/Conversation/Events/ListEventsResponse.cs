using System.Collections.Generic;

namespace Sinch.Conversation.Events
{
    public class ListEventsResponse
    {
        /// <summary>
        ///     List of <see cref="ConversationEvent"/>
        /// </summary>
        public List<ConversationEvent> Events { get; set; }

        /// <summary>
        ///     Token that should be included in the next request to fetch the next page.
        /// </summary>
        public string NextPageToken { get; set; }
    }
}
