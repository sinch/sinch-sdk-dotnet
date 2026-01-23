using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///     JSON converter for <see cref="ISmsEvent"/> that uses the "type" discriminator field
    ///     to determine the concrete event type.
    /// </summary>
    /// <remarks>
    ///     <para>Supported discriminator values:</para>
    ///     <list type="bullet">
    ///         <item><description>mo_text - <see cref="TextMessage"/></description></item>
    ///         <item><description>mo_binary - <see cref="BinaryMessage"/></description></item>
    ///         <item><description>mo_media - <see cref="MediaMessage"/></description></item>
    ///         <item><description>delivery_report_sms - <see cref="DeliveryReport"/></description></item>
    ///         <item><description>delivery_report_mms - <see cref="DeliveryReportMms"/></description></item>
    ///         <item><description>recipient_delivery_report_sms - <see cref="RecipientDeliveryReport"/></description></item>
    ///         <item><description>recipient_delivery_report_mms - <see cref="RecipientDeliveryReportMms"/></description></item>
    ///     </list>
    /// </remarks>
    public sealed class SmsEventConverter : JsonConverter<ISmsEvent>
    {
        private const string TypePropertyName = "type";

        public override ISmsEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Parse the JSON element to inspect the "type" property
            var element = JsonElement.ParseValue(ref reader);

            if (!element.TryGetProperty(TypePropertyName, out var typeProperty))
            {
                // No "type" property found, cannot determine event type
                return null;
            }

            var typeValue = typeProperty.GetString();

            return typeValue switch
            {
                // Inbound messages
                "mo_text" => element.Deserialize<TextMessage>(options),
                "mo_binary" => element.Deserialize<BinaryMessage>(options),
                "mo_media" => element.Deserialize<MediaMessage>(options),

                // Batch delivery reports
                "delivery_report_sms" => element.Deserialize<DeliveryReport>(options),
                "delivery_report_mms" => element.Deserialize<DeliveryReportMms>(options),

                // Recipient delivery reports
                "recipient_delivery_report_sms" => element.Deserialize<RecipientDeliveryReport>(options),
                "recipient_delivery_report_mms" => element.Deserialize<RecipientDeliveryReportMms>(options),

                // Unknown type
                _ => null
            };
        }

        public override void Write(Utf8JsonWriter writer, ISmsEvent value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case TextMessage textSms:
                    JsonSerializer.Serialize(writer, textSms, options);
                    break;
                case BinaryMessage binarySms:
                    JsonSerializer.Serialize(writer, binarySms, options);
                    break;
                case MediaMessage mediaSms:
                    JsonSerializer.Serialize(writer, mediaSms, options);
                    break;
                case DeliveryReport deliveryReport:
                    JsonSerializer.Serialize(writer, deliveryReport, options);
                    break;
                case DeliveryReportMms deliveryReportMms:
                    JsonSerializer.Serialize(writer, deliveryReportMms, options);
                    break;
                case RecipientDeliveryReport recipientReport:
                    JsonSerializer.Serialize(writer, recipientReport, options);
                    break;
                case RecipientDeliveryReportMms recipientReportMms:
                    JsonSerializer.Serialize(writer, recipientReportMms, options);
                    break;
                default:
                    // For unknown types, serialize as object
                    JsonSerializer.Serialize(writer, value, value.GetType(), options);
                    break;
            }
        }
    }
}
