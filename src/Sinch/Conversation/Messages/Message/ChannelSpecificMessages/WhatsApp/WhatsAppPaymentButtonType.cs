using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The type discriminator for a WhatsApp payment button.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<WhatsAppPaymentButtonType>))]
    public record WhatsAppPaymentButtonType(string Value) : EnumRecord(Value)
    {
        /// <summary>The dynamic Pix code button.</summary>
        public static readonly WhatsAppPaymentButtonType PixDynamicCode = new("pix_dynamic_code");
        /// <summary>The payment link button.</summary>
        public static readonly WhatsAppPaymentButtonType PaymentLink = new("payment_link");
        /// <summary>The Boleto button.</summary>
        public static readonly WhatsAppPaymentButtonType Boleto = new("boleto");
    }
}
