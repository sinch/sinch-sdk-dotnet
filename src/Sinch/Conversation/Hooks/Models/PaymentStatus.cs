using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     The stage the payment has reached within the payment flow.
    /// </summary>
    /// <value>The stage the payment has reached within the payment flow.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<PaymentStatus>))]
    public record PaymentStatus(string Value) : EnumRecord(Value)
    {
        public static readonly PaymentStatus PaymentStatusUnknown = new("PAYMENT_STATUS_UNKNOWN");
        public static readonly PaymentStatus PaymentStatusNew = new("PAYMENT_STATUS_NEW");
        public static readonly PaymentStatus PaymentStatusPending = new("PAYMENT_STATUS_PENDING");
        public static readonly PaymentStatus PaymentStatusCaptured = new("PAYMENT_STATUS_CAPTURED");
        public static readonly PaymentStatus PaymentStatusCanceled = new("PAYMENT_STATUS_CANCELED");
        public static readonly PaymentStatus PaymentStatusFailed = new("PAYMENT_STATUS_FAILED");
    }
}
