using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Voice.Destinations
{
    internal sealed class CalloutDestinationConverter : JsonConverter<ICalloutDestination>
    {
        public override ICalloutDestination? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var type = elem.EnumerateObject().FirstOrDefault(x => x.Name == "type").Value.GetString();

            if (string.Equals(type, "number", StringComparison.OrdinalIgnoreCase))
                return elem.Deserialize<DestinationPstn>(options);
            if (string.Equals(type, "username", StringComparison.OrdinalIgnoreCase))
                return elem.Deserialize<DestinationMxp>(options);
            if (string.Equals(type, "sip", StringComparison.OrdinalIgnoreCase))
                return elem.Deserialize<DestinationSip>(options);

            throw new JsonException($"Unknown callout destination type: '{type}'");
        }

        public override void Write(Utf8JsonWriter writer, ICalloutDestination value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case DestinationPstn pstn: JsonSerializer.Serialize(writer, pstn, options); break;
                case DestinationMxp mxp: JsonSerializer.Serialize(writer, mxp, options); break;
                case DestinationSip sip: JsonSerializer.Serialize(writer, sip, options); break;
                default: throw new ArgumentOutOfRangeException(nameof(value), $"Cannot serialize {value.GetType().Name} as ICalloutDestination");
            }
        }
    }
}
