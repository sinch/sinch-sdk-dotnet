using System.Text.Json.Serialization;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     Base type for all Voice Sinch Events.
    /// </summary>
    public abstract class VoiceSinchEvent : IVoiceSinchEvent
    {
        [JsonPropertyName("event")]
        [JsonInclude]
        internal abstract EventType Event { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("callid")]
        public string? CallId { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("conferenceId")]
        public string? ConferenceId { get; set; }

        /// <inheritdoc />
        [JsonPropertyName("version")]
        public int? Version { get; set; }

    }
}
