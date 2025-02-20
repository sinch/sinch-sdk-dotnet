using System;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    internal sealed class StartVerificationRequest
    {
        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint.
        ///     `number` is currently the only supported endpoint type.
        /// </summary>
        public Identity? Identity { get; set; }

        /// <summary>
        ///     The type of the verification request.
        /// </summary>
        public VerificationMethodEx? Method { get; set; }

        /// <summary>
        ///     Used to pass your own reference in the request for tracking purposes.
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        ///     Can be used to pass custom data in the request.
        /// </summary>
        public string? Custom { get; set; }

        /// <summary>
        ///     An optional object for flashCall verifications.
        ///     It allows you to specify dial time out parameter for flashCall.
        ///     FlashCallOptions object can be specified optionally, and only
        ///     if the verification request was triggered from your backend (no SDK client)
        ///     through an Application signed request.
        /// </summary>
        public FlashCallOptions? FlashCallOptions { get; set; }

        /// <summary>
        ///     An optional object for SMS Verification, with default values assumed for all contained values if not provided.
        /// </summary>
        public SmsOptions? SmsOptions { get; set; }

        /// <summary>
        ///     An optional object for Phone Call Verification, with default values assumed for all contained values if not provided.
        /// </summary>
        [JsonPropertyName("calloutOptions")]
        public CalloutOptions? CalloutOptions { get; set; }
    }

    internal sealed class CalloutOptions
    {
        /// <summary>
        ///     Text-To-Speech engine settings
        /// </summary>
        [JsonPropertyName("speech")]
        public SpeechEngineSetting? Speech { get; set; }
    }

    internal sealed class SpeechEngineSetting
    {
        /// <summary>
        ///     A language-region identifier according to IANA. Only a subset of those identifiers is accepted.
        /// </summary>
        public string? Locale { get; set; }
    }

    internal sealed class SmsOptions
    {
        /// <summary>
        ///     The SMS template must include a placeholder {{CODE}} where the verification code will be inserted, and it can otherwise be customized as desired.
        /// </summary>
        [JsonPropertyName("template")]
        public string? Template { get; set; }

        /// <summary>
        ///     Accepted values for the type of code to be generated are Numeric, Alpha, and Alphanumeric.
        /// </summary>
        [JsonPropertyName("codeType")]
        public CodeType? CodeType { get; set; }

        [JsonPropertyName("expiry")]
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly? Expiry { get; set; }
    }

    public sealed class FlashCallOptions
    {
        /// <summary>
        ///    The dial timeout in seconds. 
        /// </summary>
        public int DialTimeout { get; set; }
    }
}
