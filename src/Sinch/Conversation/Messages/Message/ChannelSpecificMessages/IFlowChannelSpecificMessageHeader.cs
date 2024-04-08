using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages
{
    [JsonConverter(typeof(FlowChannelSpecificMessageHeaderJsonConverter))]
    [JsonDerivedType(typeof(WhatsAppInteractiveTextHeader))]
    [JsonDerivedType(typeof(WhatsAppInteractiveDocumentHeader))]
    [JsonDerivedType(typeof(WhatsAppInteractiveImageHeader))]
    [JsonDerivedType(typeof(WhatsAppInteractiveVideoHeader))]
    public interface IFlowChannelSpecificMessageHeader
    {
        public WhatsAppHeaderType Type { get; }
    }

    public class FlowChannelSpecificMessageHeaderJsonConverter : JsonConverter<IFlowChannelSpecificMessageHeader>
    {
        public override IFlowChannelSpecificMessageHeader Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "type");
            var method = descriptor.Value.GetString();

            if (WhatsAppHeaderType.Text.Value == method)
                return elem.Deserialize<WhatsAppInteractiveTextHeader>(options);

            if (WhatsAppHeaderType.Document.Value == method)
                return elem.Deserialize<WhatsAppInteractiveDocumentHeader>(options);

            if (WhatsAppHeaderType.Video.Value == method)
                return elem.Deserialize<WhatsAppInteractiveVideoHeader>(options);

            if (WhatsAppHeaderType.Image.Value == method)
                return elem.Deserialize<WhatsAppInteractiveImageHeader>(options);

            throw new JsonException(
                $"Failed to match verification method object, got prop `{descriptor.Name}` with value `{method}`");
        }

        public override void Write(Utf8JsonWriter writer, IFlowChannelSpecificMessageHeader value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
