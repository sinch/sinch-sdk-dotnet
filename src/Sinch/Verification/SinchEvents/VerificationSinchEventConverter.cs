using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     JSON converter for <see cref="VerificationEvent"/> that uses the "event"
    ///     discriminator field to determine the concrete Verification event type.
    /// </summary>
    public sealed class VerificationSinchEventConverter : JsonConverter<VerificationEvent>
    {
        private const string EventPropertyName = "event";

        public override VerificationEvent? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var element = JsonElement.ParseValue(ref reader);

            if (!element.TryGetProperty(EventPropertyName, out var eventProperty))
            {
                throw new JsonException("Verification Sinch Event payload is missing the event discriminator.");
            }

            return eventProperty.GetString() switch
            {
                "VerificationRequestEvent" => element.Deserialize<VerificationStartEvent>(options),
                "VerificationResultEvent" => element.Deserialize<VerificationResultEvent>(options),
                "VerificationSmsDeliveredEvent" => element.Deserialize<VerificationSmsDeliveredEvent>(options),
                _ => throw new JsonException($"Unknown Verification Sinch Event type '{eventProperty.GetString()}'.")
            };
        }

        public override void Write(
            Utf8JsonWriter writer,
            VerificationEvent value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case VerificationStartEvent verificationStartEvent:
                    JsonSerializer.Serialize(writer, verificationStartEvent, options);
                    break;
                case VerificationResultEvent verificationResultEvent:
                    JsonSerializer.Serialize(writer, verificationResultEvent, options);
                    break;
                case VerificationSmsDeliveredEvent verificationSmsDeliveredEvent:
                    JsonSerializer.Serialize(writer, verificationSmsDeliveredEvent, options);
                    break;
                default:
                    JsonSerializer.Serialize(writer, value, value.GetType(), options);
                    break;
            }
        }
    }
}
