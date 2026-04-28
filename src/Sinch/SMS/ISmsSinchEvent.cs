using System.Text.Json.Serialization;

namespace Sinch.SMS
{
    /// <summary>
    ///     Base interface for all SMS Sinch events (inbound messages and delivery reports).
    ///     Used by Sinch event delivery and REST API deserialization to parse events into concrete types.
    /// </summary>
    [JsonConverter(typeof(SmsSinchEventConverter))]
    public interface ISmsSinchEvent
    {
    }
}
