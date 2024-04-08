using System;
using System.Text.Json.Serialization;

namespace Sinch.Numbers.Hooks
{
    /// <summary>
    ///     A notification of an event sent to your configured callback URL.
    /// </summary>
    public class Event
    {
        /// <summary>
        ///     The ID of the event.
        /// </summary>
        [JsonPropertyName("eventId")]
        public string EventId { get; set; }

        /// <summary>
        ///     The date and time when the callback was created and added to the callbacks queue.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///     The ID of the project to which the event belongs.
        /// </summary>
        [JsonPropertyName("projectId")]
        public string ProjectId { get; set; }

        /// <summary>
        ///     The unique identifier of the resource, depending on the resource type.
        ///     For example, a phone number, a hosting order ID, or a brand ID.
        /// </summary>
        [JsonPropertyName("resourceId")]
        public string ResourceId { get; set; }

        /// <summary>
        ///     The type of the resource. 
        /// </summary>
        [JsonPropertyName("resourceType")]
        public ResourceType ResourceType { get; set; }

        /// <summary>
        ///     The type of the event.
        /// </summary>
        [JsonPropertyName("eventType")]
        public EventType EventType { get; set; }

        /// <summary>
        ///     The status of the event
        /// </summary>
        [JsonPropertyName("status")]
        public EventStatus Status { get; set; }

        /// <summary>
        ///     If the status is FAILED, a failure code will be provided.
        ///     For numbers provisioning to SMS platform, there won't be any extra failureCode, as the result is binary.
        /// </summary>
        [JsonPropertyName("failureCode")]
        public FailureCode FailureCode { get; set; }
    }
}
