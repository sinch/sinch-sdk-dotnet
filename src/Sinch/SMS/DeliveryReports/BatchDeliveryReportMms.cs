using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    /// MMS batch delivery report Sinch event
    /// </summary>
    public sealed class BatchDeliveryReportMms : IBatchDeliveryReport
    {
        /// <summary>
        /// The delivery report type.
        /// </summary>
        [JsonPropertyName("type")]
        public DeliveryReportType Type => DeliveryReportType.Mms;

        /// <summary>
        /// Batch ID
        /// </summary>
        [JsonPropertyName("batch_id")]
        public required string BatchId { get; set; }

        /// <summary>
        /// Client reference, if any
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }

        /// <summary>
        /// Statuses for the batch
        /// </summary>
        [JsonPropertyName("statuses")]
        public required List<DeliveryReportStatus> Statuses { get; set; }

        /// <summary>
        /// Total number of messages in the batch
        /// </summary>
        [JsonPropertyName("total_message_count")]
        public required uint TotalMessageCount { get; set; }
    }
}
