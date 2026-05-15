using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Fax.SinchEvents
{
    [JsonInterfaceConverter(typeof(FaxEventConverter))]
    public interface IFaxSinchEvent
    {
        /// <summary>
        ///     The type of event delivered to the event destination.
        /// </summary>
        [JsonPropertyName("event")]
        FaxEventType Event { get; }
    }
}
