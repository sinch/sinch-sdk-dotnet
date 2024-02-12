using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Response
{
    public abstract class VerificationStartResponseBase
    {
        /// <summary>
        ///     Verification identifier used to query for status.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Available methods and actions which can be done after a successful Verification
        /// </summary>
        [JsonPropertyName("_links")]
        public List<Links> Links { get; set; }

        /// <summary>
        ///     The value of the method used for the Verification.
        /// </summary>
        public VerificationMethodEx Method { get; set; }
    }

    /// <summary>
    ///     A marker interface for VerificationResponse items.
    /// </summary>
    // Note about JsonDerivedType - it works if class has an interface property, but don't work if you try to deserialize interface itself. So, httpContent.ReadAsJson<IInterface>() will not work.
    // So I'm using JsonConverter for that
    [JsonConverter(typeof(VerificationResponseConverter))]
    public interface IStartVerificationResponse
    {
        [JsonPropertyName("method")]
        public VerificationMethodEx Method { get; set; }
    }

    public class VerificationResponseConverter : JsonConverter<IStartVerificationResponse>
    {
        public override IStartVerificationResponse Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "method");
            var method = descriptor.Value.GetString();

            if (VerificationMethodEx.Seamless.Value == method)
                return elem.Deserialize<StartDataVerificationResponse>(options);

            if (VerificationMethodEx.Sms.Value == method)
                return
                    elem.Deserialize<StartSmsVerificationResponse>(
                        options);

            if (VerificationMethodEx.FlashCall.Value == method)
                return elem.Deserialize<StartFlashCallVerificationResponse>(options);

            if (VerificationMethodEx.Callout.Value == method)
                return elem.Deserialize<StartCalloutVerificationResponse>(options);

            throw new JsonException(
                $"Failed to match verification method object, got prop `{descriptor.Name}` with value `{method}`");
        }

        public override void Write(Utf8JsonWriter writer, IStartVerificationResponse value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case StartFlashCallVerificationResponse startFlashCallVerificationResponse:
                    JsonSerializer.Serialize(writer, startFlashCallVerificationResponse, options);
                    break;
                case StartCalloutVerificationResponse startCalloutVerificationResponse:
                    JsonSerializer.Serialize(writer, startCalloutVerificationResponse, options);
                    break;
                case StartSmsVerificationResponse startSmsVerificationResponse:
                    JsonSerializer.Serialize(writer, startSmsVerificationResponse, options);
                    break;
                case StartDataVerificationResponse startDataVerificationResponse:
                    JsonSerializer.Serialize(writer, startDataVerificationResponse, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a matching class for the interface {nameof(IStartVerificationResponse)}");
            }
        }
    }
}
