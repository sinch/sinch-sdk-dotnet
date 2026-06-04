using System.Text.Json.Serialization;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     A Voice Sinch Event sent to your configured event destination.
    /// </summary>
    [JsonConverter(typeof(VoiceSinchEventConverter))]
    public interface IVoiceSinchEvent
    {
        /// <summary>
        ///     The unique ID assigned to this call.
        /// </summary>
        string? CallId { get; }

        /// <summary>
        ///     The unique Conference ID assigned to this call, if applicable.
        /// </summary>
        string? ConferenceId { get; }

        /// <summary>
        ///     The current API version.
        /// </summary>
        int? Version { get; }
    }
}
