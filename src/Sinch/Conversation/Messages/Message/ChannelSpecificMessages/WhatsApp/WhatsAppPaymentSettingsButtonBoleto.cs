using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     The Boleto payment settings button.
    /// </summary>
    public sealed class WhatsAppPaymentSettingsButtonBoleto : IWhatsAppPaymentButton
    {
        /// <inheritdoc />
        [JsonPropertyName("type")]
        public WhatsAppPaymentButtonType Type => WhatsAppPaymentButtonType.Boleto;

        /// <summary>
        ///     The Boleto digitable line which will be copied to the clipboard when the user taps the Boleto button.
        /// </summary>
        [JsonPropertyName("digitable_line")]
        public required string DigitableLine { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppPaymentSettingsButtonBoleto)} {{\n");
            sb.Append($"  {nameof(DigitableLine)}: ").Append(DigitableLine).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
