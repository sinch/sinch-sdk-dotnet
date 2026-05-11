using System.Text.Json.Serialization;

namespace Sinch.Verification.SinchEvents
{
    /// <summary>
    ///     Marker interface for all Verification Sinch Events.
    /// </summary>
    [JsonConverter(typeof(VerificationSinchEventConverter))]
    public interface IVerificationSinchEvent
    {
    }
}
