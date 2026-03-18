using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The payment link payment settings button.
    /// </summary>
    public sealed class WhatsAppPaymentSettingsButtonPaymentLink : IWhatsAppPaymentButton
    {
        /// <inheritdoc />
        [JsonPropertyName("type")]
        public WhatsAppPaymentButtonType Type => WhatsAppPaymentButtonType.PaymentLink;

        /// <summary>The payment link to be used by the buyer to pay.</summary>
        [JsonPropertyName("uri")]
        public required string Uri { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppPaymentSettingsButtonPaymentLink)} {{\n");
            sb.Append($"  {nameof(Uri)}: ").Append(Uri).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
