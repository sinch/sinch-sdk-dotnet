using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;
using Sinch.Verification.Report.Response;

namespace Sinch.Verification.Hooks
{
    /// <summary>
    ///     This callback event is a POST request to the specified verification callback URL and triggered when
    ///     a verification has been completed and the result is known.
    ///     It's used to report the verification result to the developer's backend application.
    ///     This callback event is only triggered when the verification callback URL is specified in your dashboard.
    /// </summary>
    public class VerificationResultEvent
    {
        /// <summary>
        ///     Gets or sets the ID of the verification request.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the type of the event.
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; }

        /// <summary>
        ///     Gets or sets the verification method. Must be one of the following: "sms", "flashCall", "callout".
        /// </summary>
        [JsonPropertyName("method")]
        public VerificationMethod Method { get; set; }

        /// <summary>
        ///     Gets or sets the identity information, specifying the type of endpoint that will be verified.
        /// </summary>
        [JsonPropertyName("identity")]
        public Identity Identity { get; set; }

        /// <summary>
        ///     The status of the verification request.
        /// </summary>
        [JsonPropertyName("status")]
        public VerificationStatus Status { get; set; }
        
        /// <summary>
        ///     Displays the reason why a verification has FAILED, was DENIED, or was ABORTED.
        /// </summary>
        [JsonPropertyName("reason")]
        public Reason Reason { get; set; }

        /// <summary>
        ///     The reference ID that was optionally passed together with the verification request.
        /// </summary>
        [JsonPropertyName("reference")]
        public string Reference { get; set; }
    
        /// <summary>
        ///     Free text that the client is sending, used to show if the call/SMS was intercepted or not.
        /// </summary>
        [JsonPropertyName("source")]
        public Source Source { get; set; }

        /// <summary>
        ///     A custom string that can be provided during a verification request.
        /// </summary>
        [JsonPropertyName("custom")]
        public string Custom { get; set; }
    }
}
