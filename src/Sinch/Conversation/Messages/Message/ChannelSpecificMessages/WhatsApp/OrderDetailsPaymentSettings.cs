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
        public required OrderDetailsPaymentSettingsDynamicPix DynamicPix { get; set; }

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
        public required KeyTypeEnum KeyType { get; set; }

        /// <summary>
        ///     The dynamic Pix code to be used by the buyer to pay.
        /// </summary>
        [JsonPropertyName("code")]
        public required string Code { get; set; }

        /// <summary>
        ///     Account holder name.
        /// </summary>
        [JsonPropertyName("merchant_name")]
        public required string MerchantName { get; set; }

        /// <summary>
        ///     Pix key.
        /// </summary>
        [JsonPropertyName("key")]
        public required string Key { get; set; }

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
