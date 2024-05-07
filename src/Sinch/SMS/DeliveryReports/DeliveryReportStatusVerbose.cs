using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.SMS.DeliveryReports
{
    public sealed class DeliveryReportStatusVerbose
    {
        /// <summary>
        ///     The detailed status code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        ///     The simplified status as described in <see cref="Sinch.SMS.DeliveryReports.DeliveryReportStatus" />
        /// </summary>
        [JsonPropertyName("status")]
        public DeliveryReportStatus? Status { get; set; }

        /// <summary>
        ///     The number of messages that currently has this code. Will always be at least 1
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        ///     Only for full report. A list of the phone number recipients which messages has this status code.
        /// </summary>
        [JsonPropertyName("recipients")]
        public List<string>? Recipients { get; set; }
    }
}
