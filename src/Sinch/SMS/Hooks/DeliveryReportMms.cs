using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.SMS.DeliveryReports;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    /// MMS batch delivery report webhook event
    /// </summary>
    public class DeliveryReportMms : ISmsEvent
    {
        /// <summary>
        /// Event type discriminator (always "delivery_report_mms")
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "delivery_report_mms";

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
        /// Statuses for the batch (list of status/count/recipients)
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
