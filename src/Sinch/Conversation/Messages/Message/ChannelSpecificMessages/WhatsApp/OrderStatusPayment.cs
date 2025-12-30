using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The payment order status message content
    /// </summary>
    // ref name: PaymentOrderStatusChannelSpecificMessagePayment
    public sealed class OrderStatusPayment
    {
        /// <summary>
        ///     Unique ID used to query the current payment status.
        /// </summary>
        [JsonPropertyName("reference_id")]
        public required string ReferenceId { get; set; }

        /// <summary>
        ///     Gets or Sets Order
        /// </summary>
        [JsonPropertyName("order")]
        public required OrderStatusPaymentDetails Order { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OrderStatusPayment)} {{\n");
            sb.Append($"  {nameof(ReferenceId)}: ").Append(ReferenceId).Append('\n');
            sb.Append($"  {nameof(Order)}: ").Append(Order).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
