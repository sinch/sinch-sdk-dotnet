using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    ///     Represents the delivery report type options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<DeliveryReportType>))]
    public record DeliveryReportType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Represents a delivery report for SMS.
        /// </summary>
        public static readonly DeliveryReportType Sms = new("delivery_report_sms");

        /// <summary>
        ///     Represents a delivery report for MMS.
        /// </summary>
        public static readonly DeliveryReportType Mms = new("delivery_report_mms");
    }

    /// <summary>
    ///     Represents the delivery report verbosity type options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<DeliveryReportVerbosityType>))]
    public record DeliveryReportVerbosityType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Represents a summary delivery report.
        /// </summary>
        public static readonly DeliveryReportVerbosityType Summary = new("summary");

        /// <summary>
        ///     Represents a full delivery report.
        /// </summary>
        public static readonly DeliveryReportVerbosityType Full = new("full");
    }

    /// <summary>
    ///     Represents the recipient delivery report type options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<RecipientDeliveryReportType>))]
    public record RecipientDeliveryReportType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Represents a recipient delivery report for SMS.
        /// </summary>
        public static readonly RecipientDeliveryReportType Sms = new("recipient_delivery_report_sms");
    }
}
