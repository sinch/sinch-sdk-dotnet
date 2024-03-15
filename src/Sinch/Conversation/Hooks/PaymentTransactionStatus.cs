using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     The status of the stage detailed in payment_status.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<PaymentTransactionStatus>))]
    public record PaymentTransactionStatus(string Value) : EnumRecord(Value)
    {
        
        public static readonly PaymentTransactionStatus PaymentStatusTransactionUnknown = new("PAYMENT_STATUS_TRANSACTION_UNKNOWN");
        public static readonly PaymentTransactionStatus PaymentStatusTransactionPending = new("PAYMENT_STATUS_TRANSACTION_PENDING");
        public static readonly PaymentTransactionStatus PaymentStatusTransactionFailed = new("PAYMENT_STATUS_TRANSACTION_FAILED");
        public static readonly PaymentTransactionStatus PaymentStatusTransactionSuccess = new("PAYMENT_STATUS_TRANSACTION_SUCCESS");
        public static readonly PaymentTransactionStatus PaymentStatusTransactionCanceled = new("PAYMENT_STATUS_TRANSACTION_CANCELED");
    }
}
