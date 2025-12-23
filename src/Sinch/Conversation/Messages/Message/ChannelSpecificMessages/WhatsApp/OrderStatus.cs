using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     A message type for sending WhatsApp Payment Status Requests.
    /// </summary>
    public sealed class OrderStatus : ChannelSpecificCommonProps
    {
        /// <summary>
        ///     Gets or Sets Payment
        /// </summary>
        [JsonPropertyName("payment")]
        public required OrderStatusPayment Payment { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OrderStatus)} {{\n");
            sb.Append($"  {nameof(Payment)}: ").Append(Payment).Append('\n');
            sb.Append($"  {nameof(Header)}: ").Append(Header).Append('\n');
            sb.Append($"  {nameof(Body)}: ").Append(Body).Append('\n');
            sb.Append($"  {nameof(Footer)}: ").Append(Footer).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
