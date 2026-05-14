using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Fax.SinchEvents
{
    public sealed class FaxEventConverter : JsonConverter<IFaxSinchEvent>
    {
        public override IFaxSinchEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

        public override void Write(Utf8JsonWriter writer, IFaxSinchEvent value, JsonSerializerOptions options)
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
                        $"Cannot find a matching class for the interface {nameof(IFaxSinchEvent)}");
            }
        }
    }
}
