using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     This Sinch event is a POST request to the specified verification event destination and
    ///     is triggered when a new verification request is made from the SDK client or the Verification Request API.
    ///     This Sinch event is only triggered when a verification event destination is specified in your dashboard.
    /// </summary>
    public sealed class VerificationStartEvent : VerificationEvent
    {
        /// <summary>
        ///     The amount of money and currency of the verification request.
        /// </summary>
        [JsonPropertyName("price")]
        public PriceDetail? Price { get; set; }

        /// <summary>
        ///     Allows you to set or override if provided in the API request, the SMS verification content language.
        ///     Only used with the SMS verification method.
        ///     The content language specified in the API request or in the event can be overridden
        ///     by carrier provider specific templates, due to compliance and legal requirements,
        ///     such as <see href="https://community.sinch.com/t5/SMS/Sinch-US-Short-Code-Onboarding-Overview/ta-p/7085">US shortcode requirements (pdf).</see>
        /// </summary>
        [JsonPropertyName("acceptLanguage")]
        public List<string>? AcceptLanguage { get; set; }
    }
}
