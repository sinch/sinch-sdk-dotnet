using System.Text.Json.Serialization;

namespace Sinch.SMS
{
    /// <summary>
    ///     Base interface for all SMS events (inbound messages and delivery reports).
    ///     Used by Sinch event delivery and REST API deserialization to route events to concrete types.
    /// </summary>
    [JsonConverter(typeof(SmsEventConverter))]
    public interface ISmsEvent
    {
    }
}
