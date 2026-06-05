using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     Base class for all Verification Sinch Events, containing the fields shared across every
    ///     event type as defined by the <c>VerificationEventBase</c> schema in the OAS spec.
    /// </summary>
    public abstract class VerificationSinchEvent : IVerificationSinchEvent
    {
        /// <inheritdoc />
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("event")]
        public string? Event { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("method")]
        public VerificationMethod? Method { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("identity")]
        public Identity? Identity { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("reference")]
        public string? Reference { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("custom")]
        public string? Custom { get; set; }
    }
}
