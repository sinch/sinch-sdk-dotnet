using System;
using System.Text.Json.Serialization;

namespace Sinch.Numbers.SinchEvents
{
    /// <inheritdoc />
    public sealed class NumbersSinchEvent : INumbersSinchEvent
    {
        /// <inheritdoc />
        [JsonPropertyName("eventId")]
        public string? EventId { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("projectId")]
        public string? ProjectId { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("resourceId")]
        public string? ResourceId { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("resourceType")]
        public ResourceType? ResourceType { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("eventType")]
        public EventType? EventType { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("status")]
        public EventStatus? Status { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("failureCode")]
        public FailureCode? FailureCode { get; set; }
    }
}
