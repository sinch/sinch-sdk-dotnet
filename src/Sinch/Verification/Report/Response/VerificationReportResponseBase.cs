using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public abstract class VerificationReportResponseBase
    {
        /// <summary>
        ///     The unique ID of the verification request.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     The method of the verification request.
        /// </summary>
        [JsonInclude]
        public abstract VerificationMethod Method { get; protected set; }

        /// <summary>
        ///     The status of the verification request.
        /// </summary>
        public VerificationStatus? Status { get; set; }

        /// <summary>
        ///     Displays the reason why a verification has FAILED, was DENIED, or was ABORTED.
        /// </summary>
        public Reason? Reason { get; set; }

        /// <summary>
        ///     The reference ID that was optionally passed together with the verification request.
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        ///     Free text that the client is sending, used to show if the call/SMS was intercepted or not.
        /// </summary>
        public Source? Source { get; set; }

        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint. number is currently the only supported endpoint type.
        /// </summary>
        public Identity? Identity { get; set; }
    }

    [JsonConverter(typeof(VerificationReportResponseConverter))]
    public interface IVerificationReportResponse
    {
    }

    public class VerificationReportResponseConverter : JsonConverter<IVerificationReportResponse?>
    {
        public override IVerificationReportResponse? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "method");
            var method = descriptor.Value.GetString();
            if (method == VerificationMethod.Sms.Value)
            {
                return (ReportSmsVerificationResponse?)elem.Deserialize(
                    typeof(ReportSmsVerificationResponse),
                    options);
            }

            if (method == VerificationMethod.Callout.Value)
            {
                return (ReportCalloutVerificationResponse?)elem.Deserialize(
                    typeof(ReportCalloutVerificationResponse),
                    options);
            }

            if (method == VerificationMethod.FlashCall.Value)
            {
                return (ReportFlashCallVerificationResponse?)elem.Deserialize(
                    typeof(ReportFlashCallVerificationResponse), options);
            }

            throw new JsonException($"Failed to match verification method object, got {descriptor.Name}");
        }

        public override void Write(Utf8JsonWriter writer, IVerificationReportResponse? value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case ReportFlashCallVerificationResponse flashCallVerificationReportResponse:
                    JsonSerializer.Serialize(
                        writer, flashCallVerificationReportResponse, options);
                    break;
                case ReportCalloutVerificationResponse reportCalloutVerificationResponse:
                    JsonSerializer.Serialize(
                        writer, reportCalloutVerificationResponse, options);
                    break;
                case ReportSmsVerificationResponse smsVerificationReportResponse:
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
