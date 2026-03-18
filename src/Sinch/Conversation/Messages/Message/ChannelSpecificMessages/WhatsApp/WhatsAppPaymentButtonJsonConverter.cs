using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>JSON converter for <see cref="IWhatsAppPaymentButton" /> polymorphic deserialization.</summary>
    public sealed class WhatsAppPaymentButtonJsonConverter : JsonConverter<IWhatsAppPaymentButton>
    {
        public override IWhatsAppPaymentButton Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "type");
            var typeValue = descriptor.Value.GetString();

            if (WhatsAppPaymentButtonType.PixDynamicCode.Value == typeValue)
                return elem.Deserialize<WhatsAppPaymentSettingsButtonPix>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(WhatsAppPaymentSettingsButtonPix)} deserialization result is null");

            if (WhatsAppPaymentButtonType.PaymentLink.Value == typeValue)
                return elem.Deserialize<WhatsAppPaymentSettingsButtonPaymentLink>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(WhatsAppPaymentSettingsButtonPaymentLink)} deserialization result is null");

            if (WhatsAppPaymentButtonType.Boleto.Value == typeValue)
                return elem.Deserialize<WhatsAppPaymentSettingsButtonBoleto>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(WhatsAppPaymentSettingsButtonBoleto)} deserialization result is null");

            throw new JsonException(
                $"Failed to match WhatsApp payment button type, got prop `{descriptor.Name}` with value `{typeValue}`");
        }

        public override void Write(Utf8JsonWriter writer, IWhatsAppPaymentButton value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
