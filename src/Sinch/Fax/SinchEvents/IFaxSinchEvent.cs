using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.SinchEvents
{
    [JsonInterfaceConverter(typeof(FaxEventConverter))]
    public interface IFaxSinchEvent
    {
        /// <summary>
        ///     The different events that can trigger a webhook
        /// </summary>
        [JsonPropertyName("event")]
        FaxEventType Event { get; }
    }
}
