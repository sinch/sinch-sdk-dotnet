using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.SMS.DeliveryReports;

namespace Sinch.SMS.Hooks
{
    public class DeliveryReport
    {
        /// <summary>
        ///     The ID of the batch this delivery report belongs to.
        /// </summary>
        [JsonPropertyName("batch_id")]
        public string BatchId { get; set; }

        /// <summary>
        ///     The total number of messages in the batch.
        /// </summary>
        [JsonPropertyName("statuses")]
        public List<DeliveryReportStatusVerbose> Statuses { get; set; }

        /// <summary>
        ///     The delivery report type.
        /// </summary>
        [JsonPropertyName("type")]
        public DeliveryReportType Type { get; set; }

        /// <summary>
        ///     The total number of messages in the batch.
        /// </summary>
        [JsonPropertyName("total_message_count")]
        public uint TotalMessageCount { get; set; }

        /// <summary>
        ///     The client identifier of the batch this delivery report belongs to, if set when submitting batch.
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string ClientReference { get; set; }
    }
}
