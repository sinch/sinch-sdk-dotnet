using System.Collections.Generic;

namespace Sinch.Conversation.EventDestinations
{
    public sealed class ListWebhooksResponse
    {
        /// <summary>
        ///     List of webhooks belonging to a specific project ID and app ID.
        /// </summary>
        public IEnumerable<EventDestination>? EventDestinations { get; init; }
    }
}
