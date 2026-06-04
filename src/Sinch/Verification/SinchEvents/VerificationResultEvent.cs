using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     This Sinch event is a POST request to the specified verification event destination and triggered when
    ///     a verification has been completed and the result is known.
    ///     It's used to report the verification result to the developer's backend application.
    ///     This Sinch event is only triggered when the verification event destination is specified in your dashboard.
    /// </summary>
    public sealed class VerificationResultEvent : VerificationSinchEvent
    {
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
        ///     Free text that the client is sending, used to show if the call/SMS was intercepted or not.
        /// </summary>
        [JsonPropertyName("source")]
        public Source? Source { get; set; }
    }
}
