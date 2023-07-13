using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.SMS.DeliveryReports
{
    [JsonConverter(typeof(DeliveryReportStatusEnumConverter))]
    public enum DeliveryReportStatus
    {
        /// <summary>
        ///     Message is queued within REST API system and will be dispatched according to the rate of the account.
        /// </summary>
        Queued,

        /// <summary>
        ///     Message has been dispatched and accepted for delivery by the SMSC.
        /// </summary>
        Dispatched,

        /// <summary>
        ///     Message was aborted before reaching the SMSC.
        /// </summary>
        Aborted,

        /// <summary>
        ///     Message was rejected by the SMSC.
        /// </summary>
        Rejected,

        /// <summary>
        ///     Message has been deleted. Message was deleted by a remote SMSC.
        ///     This may happen if the destination is an invalid MSISDN or opted out subscriber.
        /// </summary>
        Deleted,

        /// <summary>
        ///     Message has been delivered.
        /// </summary>
        Delivered,

        /// <summary>
        ///     Message failed to be delivered.
        /// </summary>
        Failed,

        /// <summary>
        ///     Message expired before delivery to the SMSC. This may happen if the expiry time for the message was very short.
        /// </summary>
        Expired,

        /// <summary>
        ///     Message was delivered to the SMSC but no Delivery Receipt has been received or
        ///     a Delivery Receipt that couldn't be interpreted was received.
        /// </summary>
        Unknown
    }

    internal class DeliveryReportStatusEnumConverter : JsonConverter<DeliveryReportStatus>
    {
        public override DeliveryReportStatus Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return Enum.Parse<DeliveryReportStatus>(reader.GetString()!, true);
        }

        public override void Write(Utf8JsonWriter writer, DeliveryReportStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
