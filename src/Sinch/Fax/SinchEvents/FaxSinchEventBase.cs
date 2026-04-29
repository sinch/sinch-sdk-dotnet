using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Fax.SinchEvents
{
    public abstract class FaxSinchEventBase : IFaxSinchEvent
    {
        /// <summary>
        ///     The type of the Fax Sinch event.
        /// </summary>
        [JsonPropertyName("event")]
        public abstract FaxEventType Event { get; }

        /// <summary>
        ///     Time of the event.
        /// </summary>
        [JsonPropertyName("eventTime")]
        public DateTime? EventTime { get; set; }

        /// <summary>
        ///     The fax associated with this event.
        /// </summary>
        [JsonPropertyName("fax")]
        public Faxes.Fax? Fax { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(FaxSinchEventBase)} {{\n");
            sb.Append($"  {nameof(Event)}: ").Append(Event).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(Fax)}: ").Append(Fax).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
