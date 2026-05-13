using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     This Sinch event is a POST request to the specified verification event destination and triggered when
    ///     an SMS sent for verification has been delivered to the user.
    ///     This Sinch event is only triggered when the verification event destination is specified in your dashboard.
    /// </summary>
    public sealed class VerificationSmsDeliveredEvent : IVerificationSinchEvent
    {
        /// <summary>
        ///     The ID of the verification request.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        ///     The type of the event.
        /// </summary>
        [JsonPropertyName("event")]
        public string? Event { get; set; }

        /// <summary>
        ///     The verification method.
        /// </summary>
        [JsonPropertyName("method")]
        public VerificationMethodEx? Method { get; set; }

        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint.
        ///     `number` is currently the only supported endpoint type.
        /// </summary>
        [JsonPropertyName("identity")]
        public Identity? Identity { get; set; }

        /// <summary>
        ///     The result of the SMS delivery.
        /// </summary>
        [JsonPropertyName("smsResult")]
        public SmsDeliveryResult? SmsResult { get; set; }

        /// <summary>
        ///     The reference ID that was optionally passed together with the verification request.
        /// </summary>
        [JsonPropertyName("reference")]
        public string? Reference { get; set; }

        /// <summary>
        ///     A custom string that can be provided during the verification request.
        /// </summary>
        [JsonPropertyName("custom")]
        public string? Custom { get; set; }
    }
}
