using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The payment settings.
    /// </summary>
    // ref name: PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettings
    public sealed class OrderDetailsPaymentSettings
    {
        /// <summary>
        ///     Gets or Sets DynamicPix
        /// </summary>
        [JsonPropertyName("dynamic_pix")]
#if NET7_0_OR_GREATER
        public required OrderDetailsPaymentSettingsDynamicPix DynamicPix { get; set; }
#else
        public OrderDetailsPaymentSettingsDynamicPix DynamicPix { get; set; } =
            null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OrderDetailsPaymentSettings)} {{\n");
            sb.Append($"  {nameof(DynamicPix)}: ").Append(DynamicPix).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     The dynamic Pix payment settings.
    /// </summary>
    // name ref: PaymentOrderDetailsChannelSpecificMessagePaymentPaymentSettingsDynamicPix
    public sealed class OrderDetailsPaymentSettingsDynamicPix
    {
        /// <summary>
        /// Pix key type.
        /// </summary>
        /// <value>Pix key type.</value>
        [JsonConverter(typeof(EnumRecordJsonConverter<KeyTypeEnum>))]
        public record KeyTypeEnum(string Value) : EnumRecord(Value)
        {
            public static readonly KeyTypeEnum Cpf = new("CPF");
            public static readonly KeyTypeEnum Cnpj = new("CNPJ");
            public static readonly KeyTypeEnum Email = new("EMAIL");
            public static readonly KeyTypeEnum Phone = new("PHONE");
            public static readonly KeyTypeEnum Evp = new("EVP");
        }


        /// <summary>
        /// Pix key type.
        /// </summary>
        [JsonPropertyName("key_type")]
#if NET7_0_OR_GREATER
        public required KeyTypeEnum KeyType { get; set; }
#else
        public KeyTypeEnum KeyType { get; set; } = null!;
#endif

        /// <summary>
        ///     The dynamic Pix code to be used by the buyer to pay.
        /// </summary>
        [JsonPropertyName("code")]
#if NET7_0_OR_GREATER
        public required string Code { get; set; }
#else
        public string Code { get; set; } = null!;
#endif


        /// <summary>
        ///     Account holder name.
        /// </summary>
        [JsonPropertyName("merchant_name")]
#if NET7_0_OR_GREATER
        public required string MerchantName { get; set; }
#else
        public string MerchantName { get; set; } = null!;
#endif


        /// <summary>
        ///     Pix key.
        /// </summary>
        [JsonPropertyName("key")]
#if NET7_0_OR_GREATER
        public required string Key { get; set; }
#else
        public string Key { get; set; } = null!;
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(
                $"class {nameof(OrderDetailsPaymentSettingsDynamicPix)} {{\n");
            sb.Append($"  {nameof(Code)}: ").Append(Code).Append('\n');
            sb.Append($"  {nameof(MerchantName)}: ").Append(MerchantName).Append('\n');
            sb.Append($"  {nameof(Key)}: ").Append(Key).Append('\n');
            sb.Append($"  {nameof(KeyType)}: ").Append(KeyType).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
