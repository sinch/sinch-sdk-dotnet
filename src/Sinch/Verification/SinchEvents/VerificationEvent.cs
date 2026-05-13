using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     Base class for all Verification Sinch Events, containing the fields shared across every
    ///     event type as defined by the <c>VerificationEventBase</c> schema in the OAS spec.
    /// </summary>
    [JsonConverter(typeof(VerificationSinchEventConverter))]
    public abstract class VerificationEvent
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
        public VerificationMethod? Method { get; set; }

        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint.
        ///     <c>number</c> is currently the only supported endpoint type.
        /// </summary>
        [JsonPropertyName("identity")]
        public Identity? Identity { get; set; }

        /// <summary>
        ///     Used to pass your own reference in the request for tracking purposes. Must be a unique value
        ///     for each started verification request. The value must be encodable in the URL path segment.
        ///     This value is passed to all events and returned from the status and report endpoints.
        ///     The reference can be used to check the status of verifications, like with ID or identity.
        /// </summary>
        [JsonPropertyName("reference")]
        public string? Reference { get; set; }

        /// <summary>
        ///     Can be used to pass custom data in the request. Will be passed to all events.
        ///     Max length 4096 characters, can be arbitrary text data.
        /// </summary>
        [JsonPropertyName("custom")]
        public string? Custom { get; set; }
    }
}
