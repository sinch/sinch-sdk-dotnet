using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The payment order.
    /// </summary>
    // name ref: PaymentOrderDetailsChannelSpecificMessagePaymentOrder
    public sealed class OrderDetailsPaymentOrder
    {
        /// <summary>
        ///     Unique ID of the Facebook catalog being used by the business.
        /// </summary>
        [JsonPropertyName("catalog_id")]
        public string? CatalogId { get; set; }


        /// <summary>
        ///     UTC timestamp indicating when the order should expire.  The timestamp must be given in seconds.  The minimum threshold for the timestamp is 300 seconds.
        /// </summary>
        [JsonPropertyName("expiration_time")]
        [JsonConverter(typeof(UnixTimestampJsonConverter))]
        public DateTime? ExpirationTime { get; set; }


        /// <summary>
        ///     Description of the expiration.
        /// </summary>
        [JsonPropertyName("expiration_description")]
        public string? ExpirationDescription { get; set; }


        /// <summary>
        ///     Value representing the subtotal amount of this order.
        /// </summary>
        [JsonPropertyName("subtotal_value")]
#if NET7_0_OR_GREATER
        public required int SubtotalValue { get; set; }
#else
        public int SubtotalValue { get; set; }
#endif


        /// <summary>
        ///     Value representing the tax amount for this order.
        /// </summary>
        [JsonPropertyName("tax_value")]
#if NET7_0_OR_GREATER
        public required int TaxValue { get; set; }
#else
        public int TaxValue { get; set; }
#endif


        /// <summary>
        ///     Description of the tax for this order.
        /// </summary>
        [JsonPropertyName("tax_description")]
        public string? TaxDescription { get; set; }


        /// <summary>
        ///     Value representing the shipping amount for this order.
        /// </summary>
        [JsonPropertyName("shipping_value")]
        public int ShippingValue { get; set; }


        /// <summary>
        ///     Shipping description for this order.
        /// </summary>
        [JsonPropertyName("shipping_description")]
        public string? ShippingDescription { get; set; }


        /// <summary>
        ///     Value of the discount for this order.
        /// </summary>
        [JsonPropertyName("discount_value")]
        public int DiscountValue { get; set; }


        /// <summary>
        ///     Description of the discount for this order.
        /// </summary>
        [JsonPropertyName("discount_description")]
        public string? DiscountDescription { get; set; }


        /// <summary>
        ///     Discount program name for this order.
        /// </summary>
        [JsonPropertyName("discount_program_name")]
        public string? DiscountProgramName { get; set; }


        /// <summary>
        ///     The items list for this order.
        /// </summary>
        [JsonPropertyName("items")]
#if NET7_0_OR_GREATER
        public required List<OrderDetailsPaymentOrderItems> Items { get; set; }
#else
        public List<OrderDetailsPaymentOrderItems> Items { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OrderDetailsPaymentOrder)} {{\n");
            sb.Append($"  {nameof(CatalogId)}: ").Append(CatalogId).Append('\n');
            sb.Append($"  {nameof(ExpirationTime)}: ").Append(ExpirationTime).Append('\n');
            sb.Append($"  {nameof(ExpirationDescription)}: ").Append(ExpirationDescription).Append('\n');
            sb.Append($"  {nameof(SubtotalValue)}: ").Append(SubtotalValue).Append('\n');
            sb.Append($"  {nameof(TaxValue)}: ").Append(TaxValue).Append('\n');
            sb.Append($"  {nameof(TaxDescription)}: ").Append(TaxDescription).Append('\n');
            sb.Append($"  {nameof(ShippingValue)}: ").Append(ShippingValue).Append('\n');
            sb.Append($"  {nameof(ShippingDescription)}: ").Append(ShippingDescription).Append('\n');
            sb.Append($"  {nameof(DiscountValue)}: ").Append(DiscountValue).Append('\n');
            sb.Append($"  {nameof(DiscountDescription)}: ").Append(DiscountDescription).Append('\n');
            sb.Append($"  {nameof(DiscountProgramName)}: ").Append(DiscountProgramName).Append('\n');
            sb.Append($"  {nameof(Items)}: ").Append(Items).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
