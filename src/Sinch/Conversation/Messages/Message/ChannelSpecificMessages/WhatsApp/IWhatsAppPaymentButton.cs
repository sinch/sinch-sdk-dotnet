using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     A WhatsApp payment button. Use one of the concrete implementations:
    ///     <see cref="WhatsAppPaymentSettingsButtonPix" />,
    ///     <see cref="WhatsAppPaymentSettingsButtonPaymentLink" />, or
    ///     <see cref="WhatsAppPaymentSettingsButtonBoleto" />.
    /// </summary>
    [JsonConverter(typeof(WhatsAppPaymentButtonJsonConverter))]
    [JsonDerivedType(typeof(WhatsAppPaymentSettingsButtonPix))]
    [JsonDerivedType(typeof(WhatsAppPaymentSettingsButtonPaymentLink))]
    [JsonDerivedType(typeof(WhatsAppPaymentSettingsButtonBoleto))]
    public interface IWhatsAppPaymentButton
    {
    }
}
