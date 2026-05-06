using System;
using System.Text.Json.Serialization;

namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    /// MMS recipient delivery report Sinch Event.
    /// </summary>
    public class RecipientDeliveryReportMms : IRecipientDeliveryReport
    {
        [JsonPropertyName("type")]
        public RecipientDeliveryReportType Type => RecipientDeliveryReportType.Mms;

        [JsonPropertyName("batch_id")]
        public required string BatchId { get; init; }

        [JsonPropertyName("client_reference")]
        public string? ClientReference { get; set; }

        [JsonPropertyName("at")]
        public required DateTime At { get; init; }

        /// <summary>
        ///     The detailed status code.
        ///     See <see href="https://developers.sinch.com/docs/sms/api-reference/sms/delivery-reports/delivery-report-error-codes">Delivery Report Error Codes</see>.
        /// </summary>
        [JsonPropertyName("code")]
        public required DeliveryReceiptStatusCode Code { get; init; }

        [JsonPropertyName("recipient")]
        public required string Recipient { get; init; }

        [JsonPropertyName("status")]
        public required string Status { get; init; }

        [JsonPropertyName("applied_originator")]
        public string? AppliedOriginator { get; set; }

        [JsonPropertyName("encoding")]
        public Encoding? Encoding { get; set; }

        [JsonPropertyName("number_of_message_parts")]
        public int? NumberOfMessageParts { get; set; }

        [JsonPropertyName("operator")]
        public string? Operator { get; set; }

        [JsonPropertyName("operator_status_at")]
        public DateTime? OperatorStatusAt { get; set; }
    }
}
