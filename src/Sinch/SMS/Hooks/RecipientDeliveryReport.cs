using System;
using System.Text.Json.Serialization;
using Sinch.Core;
using Sinch.SMS.DeliveryReports;

namespace Sinch.SMS.Hooks
{
    public sealed class RecipientDeliveryReport
    {
        /// <summary>
        ///     A timestamp of when the Delivery Report was created in the Sinch service.
        ///     Formatted as <see href="https://en.wikipedia.org/wiki/ISO_8601">ISO-8601</see>: YYYY-MM-DDThh:mm:ss.SSSZ.
        /// </summary>
        [JsonPropertyName("at")]
        public DateTime At { get; set; }

        /// <summary>
        ///     The ID of the batch this delivery report belongs to
        /// </summary>
        [JsonPropertyName("batch_id")]
        public string BatchId { get; set; }

        /// <summary>
        ///     The detailed status code.
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        ///     Phone number that was queried.
        /// </summary>
        [JsonPropertyName("recipient")]
        public string Recipient { get; set; }

        /// <summary>
        ///     The simplified status as described in Delivery Report Statuses.
        /// </summary>
        [JsonPropertyName("status")]
        public DeliveryReportStatus Status { get; set; }

        /// <summary>
        ///     The recipient delivery report type.
        /// </summary>
        [JsonPropertyName("type")]
        public RecipientDeliveryReportType Type { get; set; }

        /// <summary>
        ///     The default originator used for the recipient this delivery report belongs to,
        ///     if default originator pool configured and no originator set when submitting batch.
        /// </summary>
        [JsonPropertyName("applied_originator")]
        public string AppliedOriginator { get; set; }

        /// <summary>
        ///     The client identifier of the batch this delivery report belongs to, if set when submitting batch.
        /// </summary>
        [JsonPropertyName("client_reference")]
        public string ClientReference { get; set; }
        
        /// <summary>
        ///     Applied encoding for message. Present only if smart encoding is enabled.
        /// </summary>
        [JsonPropertyName("encoding")]
        public Encoding Encoding { get; set; }
        
        /// <summary>
        ///     The number of parts the message was split into.
        ///     Present only if max_number_of_message_parts parameter was set.
        /// </summary>
        [JsonPropertyName("number_of_message_parts")]
        public int? NumberOfMessageParts { get; set; }
        
        /// <summary>
        ///     The operator that was used for delivering the message to this recipient,
        ///     if enabled on the account by Sinch.
        /// </summary>
        [JsonPropertyName("operator")]
        public string Operator { get; set; }
        
        /// <summary>
        ///     A timestamp extracted from the Delivery Receipt from the originating SMSC.
        ///     Formatted as <see href="https://en.wikipedia.org/wiki/ISO_8601">ISO-8601</see>: YYYY-MM-DDThh:mm:ss.SSSZ.
        /// </summary>
        [JsonPropertyName("operator_status_at")]
        public DateTime OperatorStatusName { get; set; }
    }

    /// <summary>
    ///     Represents the encoding options for SMS.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<Encoding>))]
    public record Encoding(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Represents the GSM encoding.
        /// </summary>
        public static readonly Encoding Gsm = new("GSM");

        /// <summary>
        ///     Represents the Unicode encoding.
        /// </summary>
        public static readonly Encoding Unicode = new("UNICODE");
    }
}
