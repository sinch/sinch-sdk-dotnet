namespace Sinch.Conversation.Events
{
    public class ListEventsRequest
    {
        public string ConversationId { get; set; }

        public string ContactId { get; set; }

        public int? PageSize { get; set; }

        public string PageToken { get; set; }
    }
}
