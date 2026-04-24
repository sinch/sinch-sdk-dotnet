using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    ///     Batch-level delivery report Sinch event.
    /// </summary>
    public sealed class BatchDeliveryReportSms : IBatchDeliveryReport
    {
        /// <summary>
        ///     The delivery report type.
        /// </summary>
        [JsonPropertyName("type")]
        public DeliveryReportType Type => DeliveryReportType.Sms;

        /// <summary>
        ///     The ID of the batch this delivery report belongs to.
        /// </summary>
        [JsonPropertyName("batch_id")]
        public required string BatchId { get; set; }

        /// <summary>
        ///     The client identifier of the batch this delivery report belongs to, if set when submitting batch.
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }

        /// <summary>
        ///     Statuses for the batch
        /// </summary>
        [JsonPropertyName("statuses")]
        public required List<DeliveryReportStatusVerbose> Statuses { get; set; }

        /// <summary>
        ///     The total number of messages in the batch.
        /// </summary>
        [JsonPropertyName("total_message_count")]
        public required uint TotalMessageCount { get; set; }
    }
}
