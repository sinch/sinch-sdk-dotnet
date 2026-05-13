using System.Text.Json.Serialization;

namespace Sinch.Verification.SinchEvents
{
    public abstract class VerificationStartEventResponse
    {
        /// <summary>
        ///     Determines whether the verification can be executed.
        /// </summary>
        [JsonPropertyName("action")]
        public Action? Action { get; set; }
    }
}
