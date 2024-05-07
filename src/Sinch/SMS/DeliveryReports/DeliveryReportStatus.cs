using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.DeliveryReports
{
    /// <summary>
    ///     Represents the delivery report status options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<DeliveryReportStatus>))]
    public record DeliveryReportStatus(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Message is queued within the REST API system and will be dispatched according to the rate of the account.
        /// </summary>
        public static readonly DeliveryReportStatus Queued = new("Queued");

        /// <summary>
        ///     Message has been dispatched and accepted for delivery by the SMSC.
        /// </summary>
        public static readonly DeliveryReportStatus Dispatched = new("Dispatched");

        /// <summary>
        ///     Message was aborted before reaching the SMSC.
        /// </summary>
        public static readonly DeliveryReportStatus Aborted = new("Aborted");

        /// <summary>
        ///     Message was rejected by the SMSC.
        /// </summary>
        public static readonly DeliveryReportStatus Rejected = new("Rejected");

        /// <summary>
        ///     Message has been deleted. Message was deleted by a remote SMSC.
        ///     This may happen if the destination is an invalid MSISDN or opted-out subscriber.
        /// </summary>
        public static readonly DeliveryReportStatus Deleted = new("Deleted");

        /// <summary>
        ///     Message has been delivered.
        /// </summary>
        public static readonly DeliveryReportStatus Delivered = new("Delivered");

        /// <summary>
        ///     Message failed to be delivered.
        /// </summary>
        public static readonly DeliveryReportStatus Failed = new("Failed");

        /// <summary>
        ///     Message expired before delivery to the SMSC. This may happen if the expiry time for the message was very short.
        /// </summary>
        public static readonly DeliveryReportStatus Expired = new("Expired");

        /// <summary>
        ///     Message was delivered to the SMSC but no Delivery Receipt has been received or
        ///     a Delivery Receipt that couldn't be interpreted was received.
        /// </summary>
        public static readonly DeliveryReportStatus Unknown = new("Unknown");
    }
}
