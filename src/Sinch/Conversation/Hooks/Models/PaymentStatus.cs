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
        /// <summary>
        ///     The status value was not set. Treat it as null or not present field.
        /// </summary>
        public static readonly PaymentStatus PaymentStatusUnknown = new("PAYMENT_STATUS_UNKNOWN");

        /// <summary>
        ///     The partner sent an order_details message but the user didn't start a payment yet.
        /// </summary>
        public static readonly PaymentStatus PaymentStatusNew = new("PAYMENT_STATUS_NEW");

        /// <summary>
        ///     The user started the payment process and the payment object was created.
        /// </summary>
        public static readonly PaymentStatus PaymentStatusPending = new("PAYMENT_STATUS_PENDING");

        /// <summary>
        ///     The payment was captured.
        /// </summary>
        public static readonly PaymentStatus PaymentStatusCaptured = new("PAYMENT_STATUS_CAPTURED");

        /// <summary>
        ///     The payment was canceled by the user and no retry is possible.
        /// </summary>
        public static readonly PaymentStatus PaymentStatusCanceled = new("PAYMENT_STATUS_CANCELED");

        /// <summary>
        ///     The payment attempt failed but the user can retry.
        /// </summary>
        public static readonly PaymentStatus PaymentStatusFailed = new("PAYMENT_STATUS_FAILED");
    }
}
