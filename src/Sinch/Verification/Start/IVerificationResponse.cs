using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Verification.Start
{
    [JsonConverter(typeof(VerificationResponseConverter))]
    public interface IVerificationResponse
    {
    }

    public class VerificationResponseConverter : JsonConverter<IVerificationResponse>
    {
        public override IVerificationResponse Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "method");
            return descriptor.Value.GetString() switch
            {
                "sms" => JsonSerializer.Deserialize<SmsResponse>(ref reader),
                "callout" => JsonSerializer.Deserialize<PhoneCallResponse>(ref reader),
                "flashCall" => JsonSerializer.Deserialize<FlashCallResponse>(ref reader),
                "seamless" => JsonSerializer.Deserialize<DataResponse>(ref reader),
                _ => throw new JsonException($"Failed to match verification method object, got {descriptor.Name}")
            };
        }

        public override void Write(Utf8JsonWriter writer, IVerificationResponse value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
