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

        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint. number is currently the only supported endpoint type.
        /// </summary>
        public Identity? Identity { get; set; }
    }

    [JsonConverter(typeof(VerificationStatusResponseConverter))]
    public interface IVerificationStatusResponse
    {
    }

    public sealed class VerificationStatusResponseConverter : JsonConverter<IVerificationStatusResponse?>
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


            if (method == VerificationMethod.WhatsApp.Value)
            {
                return (WhatsAppVerificationStatusResponse?)elem.Deserialize(
                    typeof(WhatsAppVerificationStatusResponse), options);
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
                case WhatsAppVerificationStatusResponse whatsAppVerificationReportResponse:
                    JsonSerializer.Serialize(
                        writer, whatsAppVerificationReportResponse, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a proper specific type for {nameof(IVerificationStatusResponse)}");
            }
        }
    }
}
