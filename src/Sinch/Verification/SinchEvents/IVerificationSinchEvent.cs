using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     A Verification Sinch Event sent to your configured event destination.
    /// </summary>
    [JsonConverter(typeof(VerificationSinchEventConverter))]
    public interface IVerificationSinchEvent
    {
        /// <summary>
        ///     The ID of the verification request.
        /// </summary>
        string? Id { get; }

        /// <summary>
        ///     The type of the event.
        /// </summary>
        string? Event { get; }

        /// <summary>
        ///     The verification method.
        /// </summary>
        VerificationMethod? Method { get; }

        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint.
        ///     <c>number</c> is currently the only supported endpoint type.
        /// </summary>
        Identity? Identity { get; }

        /// <summary>
        ///     Used to pass your own reference in the request for tracking purposes. Must be a unique value
        ///     for each started verification request. The value must be encodable in the URL path segment.
        ///     This value is passed to all events and returned from the status and report endpoints.
        ///     The reference can be used to check the status of verifications, like with ID or identity.
        /// </summary>
        string? Reference { get; }

        /// <summary>
        ///     Can be used to pass custom data in the request. Will be passed to all events.
        ///     Max length 4096 characters, can be arbitrary text data.
        /// </summary>
        string? Custom { get; }
    }
}
