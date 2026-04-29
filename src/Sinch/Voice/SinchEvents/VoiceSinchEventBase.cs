using System.Text.Json.Serialization;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     Base class for Voice Sinch event types. Use <see cref="IVoiceSinchEvent"/> in public API.
    /// </summary>
    public abstract class VoiceSinchEventBase : IVoiceSinchEvent
    {
        [JsonPropertyName("event")]
        [JsonInclude]
        internal abstract EventType Event { get; set; }
    }
}
