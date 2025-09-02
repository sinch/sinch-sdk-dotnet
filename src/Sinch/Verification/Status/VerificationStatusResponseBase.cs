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
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        ///     The method of the verification request.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("method")]
        public virtual VerificationMethod? Method { get; protected set; }

        /// <summary>
        ///     The status of the verification request.
        /// </summary>
        [JsonPropertyName("status")]
        public VerificationStatus? Status { get; set; }

        /// <summary>
        ///     Displays the reason why a verification has FAILED, was DENIED, or was ABORTED.
        /// </summary>
        [JsonPropertyName("reason")]
        public Reason? Reason { get; set; }

        /// <summary>
        ///     The reference ID that was optionally passed together with the verification request.
        /// </summary>
        [JsonPropertyName("reference")]
        public string? Reference { get; set; }

        /// <summary>
        ///     The ID of the country to which the verification was sent.
        /// </summary>
        [JsonPropertyName("countryId")]
        public string? CountryId { get; set; }

        /// <summary>
        ///     The timestamp in UTC format. 
        /// </summary>
        [JsonPropertyName("verificationTimestamp")]
        public DateTime? VerificationTimestamp { get; set; }

        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint. number is currently the only supported endpoint type.
        /// </summary>
        [JsonPropertyName("identity")]
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
                case FlashCallVerificationStatusResponse flashCallVerificationStatusResponse:
                    JsonSerializer.Serialize(
                        writer, flashCallVerificationStatusResponse, options);
                    break;
                case CalloutVerificationStatusResponse calloutVerificationStatusResponse:
                    JsonSerializer.Serialize(
                        writer, calloutVerificationStatusResponse, options);
                    break;
                case SmsVerificationStatusResponse smsVerificationStatusResponse:
                    JsonSerializer.Serialize(
                        writer, smsVerificationStatusResponse, options);
                    break;
                case WhatsAppVerificationStatusResponse whatsAppVerificationStatusResponse:
                    JsonSerializer.Serialize(
                        writer, whatsAppVerificationStatusResponse, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a proper specific type for {nameof(IVerificationStatusResponse)}");
            }
        }
    }
}
