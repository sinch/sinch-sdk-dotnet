using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS
{
    /// <summary>
    /// Represents the delivery report options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<DeliveryReport>))]
    public record DeliveryReport(string Value) : EnumRecord(Value)
    {
        /// <summary>
        /// No delivery report callback will be sent.
        /// </summary>
        public static readonly DeliveryReport None = new("none");

        /// <summary>
        /// A single delivery report callback will be sent.
        /// </summary>
        public static readonly DeliveryReport Summary = new("summary");

        /// <summary>
        /// A single delivery report callback will be sent which includes a list of recipients per delivery status.
        /// </summary>
        public static readonly DeliveryReport Full = new("full");

        /// <summary>
        /// A delivery report callback will be sent for each status change of a message.
        /// This could result in a lot of callbacks and should be used with caution for larger batches.
        /// These delivery reports also include a timestamp of when the Delivery Report originated from the SMSC.
        /// </summary>
        public static readonly DeliveryReport PerRecipient = new("per_recipient");

        /// <summary>
        /// A delivery report callback representing the final status of a message will be sent for each recipient.
        /// This will send only one callback per recipient, compared to the multiple callbacks sent when using per_recipient.
        /// The delivery report will also include a timestamp of when it originated from the SMSC.
        /// </summary>
        public static readonly DeliveryReport PerRecipientFinal = new("per_recipient_final");
    }
}
