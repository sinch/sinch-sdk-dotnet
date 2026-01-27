using System;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    /// Incoming MMS message webhook event
    /// </summary>
    public class MediaMessage : IInboundMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "mo_media";

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("from")]
        public string From { get; set; } = string.Empty;

        [JsonPropertyName("to")]
        public string To { get; set; } = string.Empty;

        [JsonPropertyName("received_at")]
        public DateTime ReceivedAt { get; set; }

        [JsonPropertyName("sent_at")]
        public DateTime? SentAt { get; set; }

        [JsonPropertyName("operator_id")]
        public string? OperatorId { get; set; }

        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }

        [JsonPropertyName("body")]
        public MediaMessageBody MessageBody { get; set; } = new();
    }
}
