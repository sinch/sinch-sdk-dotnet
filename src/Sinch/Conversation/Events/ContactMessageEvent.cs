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
        /// The stage the payment has reached within the payment flow.
        /// </summary>
        public PaymentStatusEnum? PaymentStatus { get; set; }


        /// <summary>
        /// The status of the stage detailed in payment_status.
        /// </summary>
        public PaymentTransactionStatusEnum? PaymentTransactionStatus { get; set; }

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
    /// The status of the stage detailed in payment_status.
    /// </summary>
    /// <value>The status of the stage detailed in payment_status.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<PaymentTransactionStatusEnum>))]
    public record PaymentTransactionStatusEnum(string Value) : EnumRecord(Value)
    {
        public static readonly PaymentTransactionStatusEnum PaymentStatusTransactionUnknown =
            new("PAYMENT_STATUS_TRANSACTION_UNKNOWN");

        public static readonly PaymentTransactionStatusEnum PaymentStatusTransactionPending =
            new("PAYMENT_STATUS_TRANSACTION_PENDING");

        public static readonly PaymentTransactionStatusEnum PaymentStatusTransactionFailed =
            new("PAYMENT_STATUS_TRANSACTION_FAILED");

        public static readonly PaymentTransactionStatusEnum PaymentStatusTransactionSuccess =
            new("PAYMENT_STATUS_TRANSACTION_SUCCESS");

        public static readonly PaymentTransactionStatusEnum PaymentStatusTransactionCanceled =
            new("PAYMENT_STATUS_TRANSACTION_CANCELED");
    }

    /// <summary>
    /// The stage the payment has reached within the payment flow.
    /// </summary>
    /// <value>The stage the payment has reached within the payment flow.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<PaymentStatusEnum>))]
    public record PaymentStatusEnum(string Value) : EnumRecord(Value)
    {
        public static readonly PaymentStatusEnum PaymentStatusUnknown = new("PAYMENT_STATUS_UNKNOWN");
        public static readonly PaymentStatusEnum PaymentStatusNew = new("PAYMENT_STATUS_NEW");
        public static readonly PaymentStatusEnum PaymentStatusPending = new("PAYMENT_STATUS_PENDING");
        public static readonly PaymentStatusEnum PaymentStatusCaptured = new("PAYMENT_STATUS_CAPTURED");
        public static readonly PaymentStatusEnum PaymentStatusCanceled = new("PAYMENT_STATUS_CANCELED");
        public static readonly PaymentStatusEnum PaymentStatusFailed = new("PAYMENT_STATUS_FAILED");
    }
}
