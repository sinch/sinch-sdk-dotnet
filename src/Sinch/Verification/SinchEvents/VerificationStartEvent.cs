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
    }
}
