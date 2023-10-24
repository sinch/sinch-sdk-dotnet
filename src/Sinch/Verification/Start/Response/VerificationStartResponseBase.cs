using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Verification.Start.Response
{
    public abstract class VerificationStartResponseBase
    {
        /// <summary>
        ///     Verification identifier used to query for status.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The value of the method used for the Verification.
        /// </summary>
        public Request.VerificationMethod Method { get; set; }

        /// <summary>
        ///     Available methods and actions which can be done after a successful Verification
        /// </summary>
        [JsonPropertyName("_links")]
        public List<Links> Links { get; set; }
    }

    /// <summary>
    ///     A marker interface for VerificationResponse items.
    /// </summary>
    [JsonConverter(typeof(VerificationResponseConverter))]
    public interface IVerificationStartResponse
    {
    }

    public class VerificationResponseConverter : JsonConverter<IVerificationStartResponse>
    {
        public override IVerificationStartResponse Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "method");
            return descriptor.Value.GetString() switch
            {
                VerificationTypeInternal.Sms => (SmsVerificationStartResponse)elem.Deserialize(
                    typeof(SmsVerificationStartResponse), options),
                VerificationTypeInternal.PhoneCall => (PhoneCallVerificationStartResponse)elem.Deserialize(
                    typeof(PhoneCallVerificationStartResponse), options),
                VerificationTypeInternal.FlashCall => (FlashCallVerificationStartResponse)elem.Deserialize(
                    typeof(FlashCallVerificationStartResponse), options),
                VerificationTypeInternal.Seamless => (DataVerificationStartResponse)elem.Deserialize(
                    typeof(DataVerificationStartResponse), options),
                _ => throw new JsonException($"Failed to match verification method object, got {descriptor.Name}")
            };
        }

        public override void Write(Utf8JsonWriter writer, IVerificationStartResponse value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
