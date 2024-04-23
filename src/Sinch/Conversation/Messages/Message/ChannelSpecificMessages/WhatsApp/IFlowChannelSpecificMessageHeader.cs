using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
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
                return elem.Deserialize<WhatsAppInteractiveTextHeader>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(WhatsAppInteractiveTextHeader)} deserialization result is null");

            if (WhatsAppHeaderType.Document.Value == method)
                return elem.Deserialize<WhatsAppInteractiveDocumentHeader>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(WhatsAppInteractiveDocumentHeader)} deserialization result is null");


            if (WhatsAppHeaderType.Video.Value == method)
                return elem.Deserialize<WhatsAppInteractiveVideoHeader>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(WhatsAppInteractiveVideoHeader)} deserialization result is null");


            if (WhatsAppHeaderType.Image.Value == method)
                return elem.Deserialize<WhatsAppInteractiveImageHeader>(options) ??
                       throw new InvalidOperationException(
                           $"{nameof(WhatsAppInteractiveImageHeader)} deserialization result is null");

            throw new JsonException(
                $"Failed to match verification method object, got prop `{descriptor.Name}` with value `{method}`");
        }

        public override void Write(Utf8JsonWriter writer, IFlowChannelSpecificMessageHeader value,
            JsonSerializerOptions options)
        {
            if (value.Type == WhatsAppHeaderType.Video)
            {
                JsonSerializer.Serialize(writer, value as WhatsAppInteractiveVideoHeader, options);
            }
            else if (value.Type == WhatsAppHeaderType.Document)
            {
                JsonSerializer.Serialize(writer, value as WhatsAppInteractiveDocumentHeader, options);
            }
            else if (value.Type == WhatsAppHeaderType.Image)
            {
                JsonSerializer.Serialize(writer, value as WhatsAppInteractiveImageHeader, options);
            }
            else if (value.Type == WhatsAppHeaderType.Text)
            {
                JsonSerializer.Serialize(writer, value as WhatsAppInteractiveTextHeader, options);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Cannot serialize unknown type of {nameof(IFlowChannelSpecificMessageHeader)}");
            }
        }
    }
}
