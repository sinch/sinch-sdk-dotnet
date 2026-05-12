using System.Text.Json.Serialization;

namespace Sinch.SMS.SinchEvents
{
    /// <summary>
    ///     Base interface for all SMS events.
    /// </summary>
    [JsonConverter(typeof(SmsSinchEventConverter))]
    public interface ISmsSinchEvent
    {
    }
}
