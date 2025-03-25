using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     A message type for sending WhatsApp Payment Status Requests.
    /// </summary>
    public sealed class PaymentOrderStatusChannelSpecificMessage : ChannelSpecificCommonProps
    {
        /// <summary>
        ///     Gets or Sets Payment
        /// </summary>
        [JsonPropertyName("payment")]
#if NET7_0_OR_GREATER
        public required PaymentOrderStatusPayment Payment { get; set; }
#else
        public PaymentOrderStatusPayment Payment { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(PaymentOrderStatusChannelSpecificMessage)} {{\n");
            sb.Append($"  {nameof(Payment)}: ").Append(Payment).Append('\n');
            sb.Append($"  {nameof(Header)}: ").Append(Header).Append('\n');
            sb.Append($"  {nameof(Body)}: ").Append(Body).Append('\n');
            sb.Append($"  {nameof(Footer)}: ").Append(Footer).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
