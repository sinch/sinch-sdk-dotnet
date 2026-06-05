using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.SinchEvents
{
    public sealed class VerificationStartEventResponseSms : VerificationStartEventResponse
    {
        [JsonPropertyName("sms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Sms? Sms { get; set; }
    }

    public sealed class Sms
    {
        /// <summary>
        ///     The SMS OTP code that should be used.
        ///     By default, the Sinch dashboard will automatically generate OTP codes for SMS verification.
        ///     If you want to set your own OTP, you can specify it in the response to the Verification Request Event.
        /// </summary>
        [JsonPropertyName("code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Code { get; set; }

        /// <summary>
        ///     Selects the type of code that will be sent to the customer.
        /// </summary>
        [JsonPropertyName("codeType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SmsCodeType? CodeType { get; set; }

        /// <summary>
        ///     The expiration time for the verification process, in the format <c>HH:MM:SS</c>.
        /// </summary>
        [JsonPropertyName("expiry")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Expiry { get; set; }

        /// <summary>
        ///     The SMS verification content language. Set in the verification request.
        /// </summary>
        [JsonPropertyName("acceptLanguage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? AcceptLanguage { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new Dictionary<string, JsonElement>();
    }
}
