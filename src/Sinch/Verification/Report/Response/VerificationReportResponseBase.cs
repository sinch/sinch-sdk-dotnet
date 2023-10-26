using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;
using VerificationMethod = Sinch.Verification.Start.Request.VerificationMethod;

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
        public VerificationMethod Method { get; set; }

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
                VerificationTypeInternal.PhoneCall => (PhoneCallVerificationReportResponse)elem.Deserialize(
                    typeof(PhoneCallVerificationReportResponse),
                    options),
                VerificationTypeInternal.FlashCall => (FlashCallVerificationReportResponse)elem.Deserialize(
                    typeof(FlashCallVerificationReportResponse), options),
                _ => throw new JsonException($"Failed to match verification method object, got {descriptor.Name}")
            };
        }

        public override void Write(Utf8JsonWriter writer, IVerificationReportResponse value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case FlashCallVerificationReportResponse flashCallVerificationReportResponse:
                    JsonSerializer.Serialize(
                        writer, flashCallVerificationReportResponse, options);
                    break;
                case PhoneCallVerificationReportResponse phoneCallVerificationReportResponse:
                    JsonSerializer.Serialize(
                        writer, phoneCallVerificationReportResponse, options);
                    break;
                case SmsVerificationReportResponse smsVerificationReportResponse:
                    JsonSerializer.Serialize(
                        writer, smsVerificationReportResponse, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a proper specific type for {nameof(IVerificationReportResponse)}");
            }
        }
    }
}
