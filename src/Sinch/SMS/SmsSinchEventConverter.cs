using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Inbounds;

namespace Sinch.SMS
{
    /// <summary>
    ///     JSON converter for <see cref="ISmsSinchEvent"/> that uses the "type" discriminator field
    ///     to determine the concrete event type (inbound messages or delivery reports).
    /// </summary>
    /// <remarks>
    ///     <para>Supported discriminator values:</para>
    ///     <list type="bullet">
    ///         <item><description>mo_text - <see cref="SmsInbound"/></description></item>
    ///         <item><description>mo_binary - <see cref="BinaryInbound"/></description></item>
    ///         <item><description>mo_media - <see cref="MediaInbound"/></description></item>
    ///         <item><description>delivery_report_sms - <see cref="BatchDeliveryReportSms"/></description></item>
    ///         <item><description>delivery_report_mms - <see cref="BatchDeliveryReportMms"/></description></item>
    ///         <item><description>recipient_delivery_report_sms - <see cref="RecipientDeliveryReportSms"/></description></item>
    ///         <item><description>recipient_delivery_report_mms - <see cref="RecipientDeliveryReportMms"/></description></item>
    ///     </list>
    /// </remarks>
    public sealed class SmsSinchEventConverter : JsonConverter<ISmsSinchEvent>
    {
        private const string TypePropertyName = "type";

        public override ISmsSinchEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                // Inbound messages - route directly to concrete types
                "mo_text" => element.Deserialize<SmsInbound>(options),
                "mo_binary" => element.Deserialize<BinaryInbound>(options),
                "mo_media" => element.Deserialize<MediaInbound>(options),

                // Batch delivery reports
                "delivery_report_sms" => element.Deserialize<BatchDeliveryReportSms>(options),
                "delivery_report_mms" => element.Deserialize<BatchDeliveryReportMms>(options),

                // Recipient delivery reports
                "recipient_delivery_report_sms" => element.Deserialize<RecipientDeliveryReportSms>(options),
                "recipient_delivery_report_mms" => element.Deserialize<RecipientDeliveryReportMms>(options),

                // Unknown type
                _ => null
            };
        }

        public override void Write(Utf8JsonWriter writer, ISmsSinchEvent value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case SmsInbound smsInbound:
                    JsonSerializer.Serialize(writer, smsInbound, options);
                    break;
                case BinaryInbound binaryInbound:
                    JsonSerializer.Serialize(writer, binaryInbound, options);
                    break;
                case MediaInbound mediaInbound:
                    JsonSerializer.Serialize(writer, mediaInbound, options);
                    break;
                case BatchDeliveryReportSms deliveryReport:
                    JsonSerializer.Serialize(writer, deliveryReport, options);
                    break;
                case BatchDeliveryReportMms deliveryReportMms:
                    JsonSerializer.Serialize(writer, deliveryReportMms, options);
                    break;
                case RecipientDeliveryReportSms recipientReport:
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
