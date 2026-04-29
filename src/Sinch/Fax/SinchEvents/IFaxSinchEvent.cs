using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.SinchEvents
{
    [JsonInterfaceConverter(typeof(FaxSinchEventConverter))]
    public interface IFaxSinchEvent
    {
        /// <summary>
        ///     The type of the Fax Sinch event.
        /// </summary>
        [JsonPropertyName("event")]
        FaxEventType Event { get; }
    }
}
