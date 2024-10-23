using System;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    public abstract class CallbackEventBase : ICallbackEvent
    {
        /// <summary>
        ///     Id of the subscribed app.
        /// </summary>
        [JsonPropertyName("app_id")]
        public string? AppId { get; set; }

        /// <summary>
        ///     Timestamp marking when the channel callback was accepted/received by the Conversation API.
        /// </summary>
        [JsonPropertyName("accepted_time")]
        public DateTime? AcceptedTime { get; set; }


        /// <summary>
        ///     Timestamp of the event as provided by the underlying channels.
        /// </summary>
        [JsonPropertyName("event_time")]
        public DateTime? EventTime { get; set; }

        /// <summary>
        ///     The project ID of the app which has subscribed for the callback.
        /// </summary>
        [JsonPropertyName("project_id")]
        public string? ProjectId { get; set; }


        /// <summary>
        ///     Context-dependent metadata. Refer to specific callback&#39;s documentation for exact information provided.
        /// </summary>
        [JsonPropertyName("message_metadata")]
        public JsonNode? MessageMetadata { get; set; }


        /// <summary>
        ///     The value provided in field correlation_id of a send message request.
        /// </summary>
        [JsonPropertyName("correlation_id")]
        public string? CorrelationId { get; set; }
    }
}
