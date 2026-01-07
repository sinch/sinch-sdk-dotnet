using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The payment order details content.
    /// </summary>
    // ref name: PaymentOrderDetailsChannelSpecificMessagePayment
    public sealed class OrderDetailsPayment
    {
        /// <summary>
        /// The country/currency associated with the payment message.
        /// </summary>
        /// <value>The country/currency associated with the payment message.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<TypeEnum>))]
        public record TypeEnum(string Value) : EnumRecord(Value)
        {
            /// <summary>
            ///     Brazil
            /// </summary>
            public static readonly TypeEnum Br = new("br");
            /// <summary>
            ///     Singapore
            /// </summary>
            public static readonly TypeEnum Sg = new("sg");
        }


        /// <summary>
        /// The country/currency associated with the payment message.
        /// </summary>
        [JsonPropertyName("type")]
        public required TypeEnum Type { get; set; }

        /// <summary>
        /// The type of good associated with this order.
        /// </summary>
        [JsonPropertyName("type_of_goods")]
        public required TypeOfGoods TypeOfGoods { get; set; }

        /// <summary>
        ///     Unique reference ID.
        /// </summary>
        [JsonPropertyName("reference_id")]
        public required string ReferenceId { get; set; }

        /// <summary>
        ///     Gets or Sets PaymentSettings
        /// </summary>
        [JsonPropertyName("payment_settings")]
        public OrderDetailsPaymentSettings? PaymentSettings { get; set; }

        /// <summary>
        ///     Integer representing the total amount of the transaction.
        /// </summary>
        [JsonPropertyName("total_amount_value")]
        public required int TotalAmountValue { get; set; }

        /// <summary>
        ///     Gets or Sets Order
        /// </summary>
        [JsonPropertyName("order")]
        public required OrderDetailsPaymentOrder Order { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OrderDetailsPayment)} {{\n");
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(ReferenceId)}: ").Append(ReferenceId).Append('\n');
            sb.Append($"  {nameof(TypeOfGoods)}: ").Append(TypeOfGoods).Append('\n');
            sb.Append($"  {nameof(PaymentSettings)}: ").Append(PaymentSettings).Append('\n');
            sb.Append($"  {nameof(TotalAmountValue)}: ").Append(TotalAmountValue).Append('\n');
            sb.Append($"  {nameof(Order)}: ").Append(Order).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    /// The type of good associated with this order.
    /// </summary>
    /// <value>The type of good associated with this order.</value>
    [JsonConverter(typeof(EnumRecordJsonConverter<TypeOfGoods>))]
    public record TypeOfGoods(string Value) : EnumRecord(Value)
    {
        public static readonly TypeOfGoods DigitalGoods = new("digital-goods");
        public static readonly TypeOfGoods PhysicalGoods = new("physical-goods");
    }
}
