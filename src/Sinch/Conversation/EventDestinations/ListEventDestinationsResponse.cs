using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.EventDestinations
{
    public sealed class ListEventDestinationsResponse
    {
        /// <summary>
        ///     List of event destinations belonging to a specific project ID and app ID.
        /// </summary>
        [JsonPropertyName("webhooks")]
        public IEnumerable<EventDestination>? EventDestinations { get; init; }
    }
}
