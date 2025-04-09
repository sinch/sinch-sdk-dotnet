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
#if NET7_0_OR_GREATER
        public required string RetailerId { get; set; }
#else
        public string RetailerId { get; set; } = null!;
#endif


        /// <summary>
        ///     Item&#39;s name as displayed to the user.
        /// </summary>
        [JsonPropertyName("name")]
#if NET7_0_OR_GREATER
        public required string Name { get; set; }
#else
        public string Name { get; set; } = null!;
#endif


        /// <summary>
        ///     Price per item.
        /// </summary>
        [JsonPropertyName("amount_value")]
#if NET7_0_OR_GREATER
        public required int AmountValue { get; set; }
#else
        public int AmountValue { get; set; }
#endif


        /// <summary>
        ///     Number of items in this order.
        /// </summary>
        [JsonPropertyName("quantity")]
#if NET7_0_OR_GREATER
        public required int Quantity { get; set; }
#else
        public int Quantity { get; set; }
#endif


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
