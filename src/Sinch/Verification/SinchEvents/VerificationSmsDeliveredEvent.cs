using System.Text.Json.Serialization;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     This Sinch event is a POST request to the specified verification event destination and triggered when
    ///     an SMS sent for verification has been delivered to the user.
    ///     This Sinch event is only triggered when the verification event destination is specified in your dashboard.
    /// </summary>
    public sealed class VerificationSmsDeliveredEvent : VerificationSinchEvent
    {
        /// <summary>
        ///     The result of the SMS delivery.
        /// </summary>
        [JsonPropertyName("smsResult")]
        public SmsDeliveryResult? SmsResult { get; set; }
    }
}
