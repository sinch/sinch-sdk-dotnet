using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public sealed class StartSmsVerificationRequest : StartVerificationRequestBase
    {
        /// <summary>
        ///     The type of the verification request. Set to SMS.
        /// </summary>
        [JsonInclude]
        public override VerificationMethodEx Method { get; } = VerificationMethodEx.Sms;

        /// <summary>
        ///     Value of [Accept-Language](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language) header is used to determine the language of an SMS message.
        /// </summary>
        [JsonIgnore]
        public string? AcceptLanguage { get; set; }
    }
}
