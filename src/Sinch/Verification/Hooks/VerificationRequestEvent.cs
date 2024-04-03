using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Hooks
{
    /// <summary>
    ///     This callback event is a POST request to the specified verification callback URL and
    ///     is triggered when a new verification request is made from the SDK client or the Verification Request API.
    ///     This callback event is only triggered when a verification callback URL is specified in your dashboard.
    /// </summary>
    public class VerificationRequestEvent
    {
        /// <summary>
        ///     The ID of the verification request.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        ///     The type of the event.
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; }

        /// <summary>
        ///     The verification method.
        /// </summary>
        [JsonPropertyName("method")]
        public VerificationMethod Method { get; set; }

        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint.
        ///     `number` is currently the only supported endpoint type.
        /// </summary>
        [JsonPropertyName("identity")]
        public Identity Identity { get; set; }

        /// <summary>
        ///     The amount of money and currency of the verification request.
        /// </summary>
        [JsonPropertyName("price")]
        public PriceDetail Price { get; set; }

        /// <summary>
        ///     Used to pass your own reference in the request for tracking purposes.
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        /// <summary>
        ///     Can be used to pass custom data in the request.
        /// </summary>
        [JsonPropertyName("custom")]
        public string Custom { get; set; }

        /// <summary>
        ///     Allows you to set or override if provided in the API request, the SMS verification content language.
        ///     Only used with the SMS verification method.
        ///     The content language specified in the API request or in the callback can be overridden
        ///     by carrier provider specific templates, due to compliance and legal requirements,
        ///     such as <see href="https://community.sinch.com/t5/SMS/Sinch-US-Short-Code-Onboarding-Overview/ta-p/7085">US shortcode requirements (pdf).</see>
        /// </summary>
        [JsonPropertyName("acceptLanguage")]
        public List<string> AcceptLanguage { get; set; }
    }
}
