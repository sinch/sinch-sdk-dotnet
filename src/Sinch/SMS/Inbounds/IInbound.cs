using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.SMS.Inbounds
{
    /// <summary>
    ///     Marker interface for Inbound types
    /// </summary>
    [JsonInterfaceConverter(typeof(InboundJsonConverter))]
    public interface IInbound
    {
    }

    public class InboundJsonConverter : JsonConverter<IInbound>
    {
        public override IInbound? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "type");
            var method = descriptor.Value.GetString();

            if (SmsType.Text.Value == method)
                return elem.Deserialize<SmsInbound>(options);

            if (SmsType.Binary.Value == method)
                return elem.Deserialize<BinaryInbound>(options);

            throw new JsonException(
                $"Failed to match verification method object, got prop `{descriptor.Name}` with value `{method}`");
        }

        public override void Write(Utf8JsonWriter writer, IInbound value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            if (type == typeof(SmsInbound))
            {
                JsonSerializer.Serialize(writer, value as SmsInbound, options);
            }

            if (type == typeof(BinaryInbound))
            {
                JsonSerializer.Serialize(writer, value as BinaryInbound, options);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Cannot serialize unknown type of {nameof(IInbound)}");
            }
        }
    }
}
