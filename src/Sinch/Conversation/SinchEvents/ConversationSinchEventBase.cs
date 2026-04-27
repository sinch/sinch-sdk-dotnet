using System;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.SinchEvents
{
    public abstract class ConversationSinchEventBase : IConversationSinchEvent
    {
        /// <summary>
        ///     Id of the subscribed app.
        /// </summary>
        [JsonPropertyName("app_id")]
        public required string AppId { get; set; }

        /// <summary>
        ///     Timestamp marking when the channel event was accepted/received by the Conversation API.
        /// </summary>
        [JsonPropertyName("accepted_time")]
        public DateTime? AcceptedTime { get; set; }

        /// <summary>
        ///     Timestamp of the event as provided by the underlying channels.
        /// </summary>
        [JsonPropertyName("event_time")]
        public DateTime? EventTime { get; set; }

        /// <summary>
        ///     The project ID of the app which has subscribed for the Sinch event.
        /// </summary>
        [JsonPropertyName("project_id")]
        public required string ProjectId { get; set; }

        /// <summary>
        ///     Context-dependent metadata. Refer to specific Sinch event&#39;s documentation for exact information provided.
        /// </summary>
        [JsonPropertyName("message_metadata")]
        public string? MessageMetadata { get; set; }

        /// <summary>
        ///     The value provided in field correlation_id of a send message request.
        /// </summary>
        [JsonPropertyName("correlation_id")]
        public string? CorrelationId { get; set; }
    }
}
