using System.Text.Json.Serialization;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     Base type for all Voice Sinch Events.
    /// </summary>
    [JsonConverter(typeof(VoiceSinchEventConverter))]
    public abstract class VoiceSinchEvent
    {
        [JsonPropertyName("event")]
        [JsonInclude]
        internal abstract EventType Event { get; set; }
    }
}
