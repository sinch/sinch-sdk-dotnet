using System.Text;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The dynamic Pix payment settings button.
    /// </summary>
    // ref name: WhatsAppPaymentSettingsButtonPix
    public sealed class WhatsAppPaymentSettingsButtonPix : IWhatsAppPaymentButton
    {
        /// <summary>
        ///     Pix key type.
        /// </summary>
        [JsonConverter(typeof(EnumRecordJsonConverter<PixKeyType>))]
        public record PixKeyType(string Value) : EnumRecord(Value)
        {
            public static readonly PixKeyType Cpf = new("CPF");
            public static readonly PixKeyType Cnpj = new("CNPJ");
            public static readonly PixKeyType Email = new("EMAIL");
            public static readonly PixKeyType Phone = new("PHONE");
            public static readonly PixKeyType Evp = new("EVP");
        }

        /// <inheritdoc />
        [JsonPropertyName("type")]
        public WhatsAppPaymentButtonType Type { get; private set; } =
            WhatsAppPaymentButtonType.PixDynamicCode;

        /// <summary>The dynamic Pix code to be used by the buyer to pay.</summary>
        [JsonPropertyName("code")]
        public required string Code { get; set; }

        /// <summary>Account holder name.</summary>
        [JsonPropertyName("merchant_name")]
        public required string MerchantName { get; set; }

        /// <summary>Pix key.</summary>
        [JsonPropertyName("key")]
        public required string Key { get; set; }

        /// <summary>Pix key type.</summary>
        [JsonPropertyName("key_type")]
        public required PixKeyType KeyType { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppPaymentSettingsButtonPix)} {{\n");
            sb.Append($"  {nameof(Code)}: ").Append(Code).Append('\n');
            sb.Append($"  {nameof(MerchantName)}: ").Append(MerchantName).Append('\n');
            sb.Append($"  {nameof(Key)}: ").Append(Key).Append('\n');
            sb.Append($"  {nameof(KeyType)}: ").Append(KeyType).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
