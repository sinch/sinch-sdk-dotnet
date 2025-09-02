using System.Text.Json.Serialization;

namespace Sinch.Verification.Start.Response
{
    public sealed class StartSmsVerificationResponse : VerificationStartResponseBase, IStartVerificationResponse
    {
        /// <summary>
        ///     The response contains the template of the SMS to be expected and intercepted.
        /// </summary>
        [JsonPropertyName("sms")]
        public SmsInfo? Sms { get; set; }
    }

    /// <summary>
    ///     The response contains the template of the SMS to be expected and intercepted.
    /// </summary>
    public sealed class SmsInfo
    {
        /// <summary>
        ///     The expected template for the SMS response.
        /// </summary>
        [JsonPropertyName("template")]
        public string? Template { get; set; }

        /// <summary>
        ///     The amount of time in seconds that the client should wait for the SMS.
        /// </summary>
        [JsonPropertyName("interceptionTimeout")]
        public int? InterceptionTimeout { get; set; }
    }
}
