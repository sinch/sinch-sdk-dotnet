using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    /// MMS batch delivery report webhook event
    /// </summary>
    public sealed class BatchDeliveryReportMms : IBatchDeliveryReport
    {
        /// <summary>
        /// The delivery report type.
        /// </summary>
        [JsonPropertyName("type")]
        public required DeliveryReportType Type { get; init; } = DeliveryReportType.Mms;

        /// <summary>
        /// Batch ID
        /// </summary>
        [JsonPropertyName("batch_id")]
        public string BatchId { get; set; } = string.Empty;

        /// <summary>
        /// Client reference, if any
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }

        /// <summary>
        /// Statuses for the batch
        /// </summary>
        [JsonPropertyName("statuses")]
        public List<DeliveryReportStatus>? Statuses { get; set; }

        /// <summary>
        /// Total number of messages in the batch
        /// </summary>
        [JsonPropertyName("total_message_count")]
        public int TotalMessageCount { get; set; }
    }
}
