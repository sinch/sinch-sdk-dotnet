using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     PaymentOrderDetailsChannelSpecificMessagePaymentOrderItems
    /// </summary>
    // ref name: PaymentOrderDetailsChannelSpecificMessagePaymentOrderItems
    public sealed class OrderDetailsPaymentOrderItems
    {
        /// <summary>
        ///     Unique ID of the retailer.
        /// </summary>
        [JsonPropertyName("retailer_id")]
        public required string RetailerId { get; set; }


        /// <summary>
        ///     Item&#39;s name as displayed to the user.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }


        /// <summary>
        ///     Price per item.
        /// </summary>
        [JsonPropertyName("amount_value")]
        public required int AmountValue { get; set; }


        /// <summary>
        ///     Number of items in this order.
        /// </summary>
        [JsonPropertyName("quantity")]
        public required int Quantity { get; set; }


        /// <summary>
        ///     Discounted price per item.
        /// </summary>
        [JsonPropertyName("sale_amount_value")]
        public int SaleAmountValue { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OrderDetailsPaymentOrderItems)} {{\n");
            sb.Append($"  {nameof(RetailerId)}: ").Append(RetailerId).Append('\n');
            sb.Append($"  {nameof(Name)}: ").Append(Name).Append('\n');
            sb.Append($"  {nameof(AmountValue)}: ").Append(AmountValue).Append('\n');
            sb.Append($"  {nameof(Quantity)}: ").Append(Quantity).Append('\n');
            sb.Append($"  {nameof(SaleAmountValue)}: ").Append(SaleAmountValue).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
