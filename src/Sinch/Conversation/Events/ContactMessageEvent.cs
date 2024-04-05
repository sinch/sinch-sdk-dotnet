using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Events
{
    public class ContactMessageEvent
    {
        // Thank you System.Text.Json -_-
        [JsonConstructor]
        [Obsolete("Needed for System.Text.Json", true)]
        public ContactMessageEvent()
        {
        }

        public ContactMessageEvent(PaymentStatusUpdateEvent paymentStatusUpdateEvent)
        {
            PaymentStatusUpdateEvent = paymentStatusUpdateEvent;
        }

        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PaymentStatusUpdateEvent PaymentStatusUpdateEvent { get; private set; }
    }


    /// <summary>
    ///     Object reflecting the current state of a particular payment flow.
    /// </summary>
    public sealed class PaymentStatusUpdateEvent
    {
        /// <summary>
        ///     The stage the payment has reached within the payment flow.
        /// </summary>
        public PaymentStatus? PaymentStatus { get; set; }


        /// <summary>
        ///     The status of the stage detailed in payment_status.
        /// </summary>
        public PaymentTransactionStatus? PaymentTransactionStatus { get; set; }

        /// <summary>
        ///     Unique identifier for the corresponding payment of a particular order.
        /// </summary>
        public string ReferenceId { get; set; }


        /// <summary>
        ///     Unique identifier of the payment_transaction_status.
        /// </summary>
        public string PaymentTransactionId { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PaymentStatusUpdateEvent {\n");
            sb.Append("  ReferenceId: ").Append(ReferenceId).Append("\n");
            sb.Append("  PaymentStatus: ").Append(PaymentStatus).Append("\n");
            sb.Append("  PaymentTransactionStatus: ").Append(PaymentTransactionStatus).Append("\n");
            sb.Append("  PaymentTransactionId: ").Append(PaymentTransactionId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     The status of the stage detailed in payment_status.
    /// </summary>
    /// <value>The status of the stage detailed in payment_status.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<PaymentTransactionStatus>))]
    public record PaymentTransactionStatus(string Value) : EnumRecord(Value)
    {
        public static readonly PaymentTransactionStatus PaymentStatusTransactionUnknown =
            new("PAYMENT_STATUS_TRANSACTION_UNKNOWN");

        public static readonly PaymentTransactionStatus PaymentStatusTransactionPending =
            new("PAYMENT_STATUS_TRANSACTION_PENDING");

        public static readonly PaymentTransactionStatus PaymentStatusTransactionFailed =
            new("PAYMENT_STATUS_TRANSACTION_FAILED");

        public static readonly PaymentTransactionStatus PaymentStatusTransactionSuccess =
            new("PAYMENT_STATUS_TRANSACTION_SUCCESS");

        public static readonly PaymentTransactionStatus PaymentStatusTransactionCanceled =
            new("PAYMENT_STATUS_TRANSACTION_CANCELED");
    }

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
