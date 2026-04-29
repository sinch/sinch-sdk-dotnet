using Sinch.Core;

namespace Sinch.Voice.SinchEvents
{
    /// <summary>
    ///     Marker interface for Voice Sinch event types.
    /// </summary>
    [JsonInterfaceConverter(typeof(VoiceSinchEventConverter))]
    public interface IVoiceSinchEvent
    {
    }
}
