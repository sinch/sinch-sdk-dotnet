using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The payment order details content.
    /// </summary>
    public sealed class PaymentOrderDetailsChannelSpecificMessagePayment
    {
        /// <summary>
        /// The country/currency associated with the payment message.
        /// </summary>
        /// <value>The country/currency associated with the payment message.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<TypeEnum>))]
        public record TypeEnum(string Value) : EnumRecord(Value)
        {
            public static readonly TypeEnum Br = new("br");
        }


        /// <summary>
        /// The country/currency associated with the payment message.
        /// </summary>
        [JsonPropertyName("type")]
#if NET7_0_OR_GREATER
        public required TypeEnum Type { get; set; }
#else
        public TypeEnum Type { get; set; } = null!;
#endif

       


        /// <summary>
        /// The type of good associated with this order.
        /// </summary>
        [JsonPropertyName("type_of_goods")]
#if NET7_0_OR_GREATER
        public required TypeOfGoods TypeOfGoods { get; set; }
#else
        public TypeOfGoods TypeOfGoods { get; set; } = null!;
#endif

        /// <summary>
        ///     Unique reference ID.
        /// </summary>
        [JsonPropertyName("reference_id")]
#if NET7_0_OR_GREATER
        public required string ReferenceId { get; set; }
#else
        public string ReferenceId { get; set; } = null!;
#endif


        /// <summary>
        ///     Gets or Sets PaymentSettings
        /// </summary>
        [JsonPropertyName("payment_settings")]
        public PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettings? PaymentSettings { get; set; }


        /// <summary>
        ///     Integer representing the total amount of the transaction.
        /// </summary>
        [JsonPropertyName("total_amount_value")]
#if NET7_0_OR_GREATER
        public required int TotalAmountValue { get; set; }
#else
        public int TotalAmountValue { get; set; }
#endif


        /// <summary>
        ///     Gets or Sets Order
        /// </summary>
        [JsonPropertyName("order")]
#if NET7_0_OR_GREATER
        public required PaymentOrderDetailsChannelSpecificMessagePaymentOrder Order { get; set; }
#else
        public PaymentOrderDetailsChannelSpecificMessagePaymentOrder Order { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(PaymentOrderDetailsChannelSpecificMessagePayment)} {{\n");
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
