using System;
using System.Text.Json.Serialization;

namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    /// MMS recipient delivery report webhook event
    /// </summary>
    public class RecipientDeliveryReportMms : IRecipientDeliveryReport
    {
        [JsonPropertyName("type")]
        public RecipientDeliveryReportType Type { get; set; } = RecipientDeliveryReportType.Mms;

        [JsonPropertyName("batch_id")]
        public string BatchId { get; set; } = string.Empty;

        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }

        [JsonPropertyName("at")]
        public DateTime At { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("recipient")]
        public string Recipient { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("applied_originator")]
        public string? AppliedOriginator { get; set; }

        [JsonPropertyName("encoding")]
        public string? Encoding { get; set; }

        [JsonPropertyName("number_of_message_parts")]
        public int? NumberOfMessageParts { get; set; }

        [JsonPropertyName("operator")]
        public string? Operator { get; set; }

        [JsonPropertyName("operator_status_at")]
        public DateTime? OperatorStatusAt { get; set; }
    }
}
