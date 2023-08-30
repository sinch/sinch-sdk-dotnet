using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS
{
    [JsonConverter(typeof(SinchEnumConverter<DeliveryReport>))]
    public enum DeliveryReport
    {
        /// <summary>
        ///     No delivery report callback will be sent.
        /// </summary>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        ///     A single delivery report callback will be sent.
        /// </summary>
        [EnumMember(Value = "summary")]
        Summary,

        /// <summary>
        ///     A single delivery report callback will be sent which includes a list of recipients per delivery status.
        /// </summary>
        [EnumMember(Value = "full")]
        Full,

        /// <summary>
        ///     A delivery report callback will be sent for each status change of a message.
        ///     This could result in a lot of callbacks and should be used with caution for larger batches.
        ///     These delivery reports also include a timestamp of when the Delivery Report originated from the SMSC.
        /// </summary>
        [EnumMember(Value = "per_recipient")]
        PerRecipient,

        /// <summary>
        ///     A delivery report callback representing the final status of a message will be sent for each recipient.
        ///     This will send only one callback per recipient, compared to the multiple callbacks sent when using per_recipient.
        ///     The delivery report will also include a timestamp of when it originated from the SMSC.
        /// </summary>
        [EnumMember(Value = "per_recipient_final")]
        PerRecipientFinal
    }
}
