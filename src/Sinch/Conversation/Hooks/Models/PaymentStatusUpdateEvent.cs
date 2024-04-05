using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     Object reflecting the current state of a particular payment flow.
    /// </summary>
    public sealed class PaymentStatusUpdateEvent
    {
        /// <summary>
        /// The stage the payment has reached within the payment flow.
        /// </summary>
        [JsonPropertyName("payment_status")]

        public PaymentStatus PaymentStatus { get; set; }


        /// <summary>
        /// The status of the stage detailed in payment_status.
        /// </summary>
        [JsonPropertyName("payment_transaction_status")]
        public PaymentTransactionStatus? PaymentTransactionStatus { get; set; }

        /// <summary>
        ///     Unique identifier for the corresponding payment of a particular order.
        /// </summary>
        [JsonPropertyName("reference_id")]
        public string ReferenceId { get; set; }


        /// <summary>
        ///     Unique identifier of the payment_transaction_status.
        /// </summary>
        [JsonPropertyName("payment_transaction_id")]
        public string PaymentTransactionId { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(PaymentStatusUpdateEvent)} {{\n");
            sb.Append($"  {nameof(ReferenceId)}: ").Append(ReferenceId).Append('\n');
            sb.Append($"  {nameof(PaymentStatus)}: ").Append(PaymentStatus).Append('\n');
            sb.Append($"  {nameof(PaymentTransactionStatus)}: ").Append(PaymentTransactionStatus).Append('\n');
            sb.Append($"  {nameof(PaymentTransactionId)}: ").Append(PaymentTransactionId).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
