using System.Text.Json.Serialization;

namespace Sinch.SMS.SinchEvents
{
    /// <summary>
    ///     Base interface for all SMS events (inbound messages and delivery reports).
    ///     Used by Sinch Events and REST API deserialization to route events to concrete types.
    /// </summary>
    [JsonConverter(typeof(SmsEventConverter))]
    public interface ISmsEvent
    {
    }
}
