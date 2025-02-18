using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;
using Sinch.Fax.Faxes;

namespace Sinch.Fax.Hooks
{
    [JsonInterfaceConverter(typeof(FaxEventConverter))]
    public interface IFaxEvent
    {
        /// <summary>
        ///     The different events that can trigger a webhook
        /// </summary>
        [JsonPropertyName("event")]
        public FaxEventType Event { get; }
    }

    public abstract class GenericFaxEvent
    {
        /// <summary>
        ///     The different events that can trigger a webhook
        /// </summary>
        [JsonPropertyName("event")]
        public abstract FaxEventType Event { get; }

        /// <summary>
        ///     Time of the event.
        /// </summary>
        [JsonPropertyName("eventTime")]
        public DateTime? EventTime { get; set; }


        /// <summary>
        ///     Gets or Sets Fax
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
            sb.Append($"class {nameof(IFaxEvent)} {{\n");
            sb.Append($"  {nameof(Event)}: ").Append(Event).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(Fax)}: ").Append(Fax).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    [JsonConverter(typeof(EnumRecordJsonConverter<FaxEventType>))]
    public record FaxEventType(string Value) : EnumRecord(Value)
    {
        public static readonly FaxEventType IncomingFax = new("INCOMING_FAX");
        public static readonly FaxEventType CompletedFax = new("FAX_COMPLETED");
    }

    public sealed class FaxEventConverter : JsonConverter<IFaxEvent>
    {
        public override IFaxEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "event");
            var type = descriptor.Value.GetString();

            if (type == FaxEventType.IncomingFax.Value)
            {
                return elem.Deserialize<IncomingFaxEvent>(options);
            }

            if (type == FaxEventType.CompletedFax.Value)
            {
                return elem.Deserialize<CompletedFaxEvent>(options);
            }

            throw new JsonException($"Failed to match verification method object, got {descriptor.Name}");
        }

        public override void Write(Utf8JsonWriter writer, IFaxEvent value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case IncomingFaxEvent incomingFaxEvent:
                    JsonSerializer.Serialize(writer, incomingFaxEvent, options);
                    break;
                case CompletedFaxEvent completedFaxEvent:
                    JsonSerializer.Serialize(writer, completedFaxEvent, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Cannot find a matching class for the interface {nameof(IFaxEvent)}");
            }
        }
    }
}
