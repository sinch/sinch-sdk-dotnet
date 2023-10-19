using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Verification.Report.Response
{
    public abstract class VerificationReportResponseBase
    {
        /// <summary>
        ///     The unique ID of the verification request.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The method of the verification request.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        ///     The status of the verification request.
        /// </summary>
        public VerificationStatus Status { get; set; }

        /// <summary>
        ///     Displays the reason why a verification has FAILED, was DENIED, or was ABORTED.
        /// </summary>
        public Reason Reason { get; set; }

        /// <summary>
        ///     The reference ID that was optionally passed together with the verification request.
        /// </summary>
        public string Reference { get; set; }
    }

    [JsonConverter(typeof(VerificationReportResponseConverter))]
    public interface IVerificationReportResponse
    {
    }

    public class VerificationReportResponseConverter : JsonConverter<IVerificationReportResponse>
    {
        public override IVerificationReportResponse Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "method");
            return descriptor.Value.GetString() switch
            {
                VerificationTypeInternal.Sms => (SmsVerificationReportResponse)elem.Deserialize(
                    typeof(SmsVerificationReportResponse),
                    options),
                VerificationTypeInternal.PhoneCall => (PhoneVerificationReportResponse)elem.Deserialize(
                    typeof(PhoneVerificationReportResponse),
                    options),
                VerificationTypeInternal.FlashCall => (FlashCallVerificationReportResponse)elem.Deserialize(
                    typeof(FlashCallVerificationReportResponse), options),
                _ => throw new JsonException($"Failed to match verification method object, got {descriptor.Name}")
            };
        }

        public override void Write(Utf8JsonWriter writer, IVerificationReportResponse value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
