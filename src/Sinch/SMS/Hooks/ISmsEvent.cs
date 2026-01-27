using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    ///     Base class for all WebHook event's class
    /// </summary>
    /// <seealso cref="SmsWebhooks"/>
    [JsonConverter(typeof(SmsEventConverter))]
    public interface ISmsEvent
    {
    }
}
