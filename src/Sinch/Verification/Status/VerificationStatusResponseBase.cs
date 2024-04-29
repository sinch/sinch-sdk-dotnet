using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Status
{
    public abstract class VerificationStatusResponseBase
    {
        /// <summary>
        ///     The unique ID of the verification request.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     The method of the verification request.
        /// </summary>
        [JsonInclude]
        public virtual VerificationMethod? Method { get; protected set; }

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
        ///     The ID of the country to which the verification was sent.
        /// </summary>
        public string? CountryId { get; set; }

        /// <summary>
        ///     The timestamp in UTC format. 
        /// </summary>
        public DateTime? VerificationTimestamp { get; set; }
    }

    [JsonConverter(typeof(VerificationStatusResponseConverter))]
    public interface IVerificationStatusResponse
    {
    }

    public class VerificationStatusResponseConverter : JsonConverter<IVerificationStatusResponse?>
    {
        public override IVerificationStatusResponse? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "method");
            var method = descriptor.Value.GetString();
            if (method == VerificationMethod.Sms.Value)
            {
                return (SmsVerificationStatusResponse?)elem.Deserialize(
                    typeof(SmsVerificationStatusResponse),
                    options);
            }

            if (method == VerificationMethod.Callout.Value)
            {
                return (CalloutVerificationStatusResponse?)elem.Deserialize(
                    typeof(CalloutVerificationStatusResponse),
                    options);
            }

            if (method == VerificationMethod.FlashCall.Value)
            {
                return (FlashCallVerificationStatusResponse?)elem.Deserialize(
                    typeof(FlashCallVerificationStatusResponse), options);
            }

            throw new JsonException($"Failed to match verification method object, got {descriptor.Name}");
        }

        public override void Write(Utf8JsonWriter writer, IVerificationStatusResponse? value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case FlashCallVerificationStatusResponse flashCallVerificationReportResponse:
                    JsonSerializer.Serialize(
                        writer, flashCallVerificationReportResponse, options);
                    break;
                case CalloutVerificationStatusResponse reportCalloutVerificationResponse:
                    JsonSerializer.Serialize(
                        writer, reportCalloutVerificationResponse, options);
                    break;
                case SmsVerificationStatusResponse smsVerificationReportResponse:
                    JsonSerializer.Serialize(
                        writer, smsVerificationReportResponse, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a proper specific type for {nameof(IVerificationStatusResponse)}");
            }
        }
    }
}
