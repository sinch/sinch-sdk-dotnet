using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    [JsonInterfaceConverter(typeof(OmniMessageOverrideJsonConverter))]
    public interface IOmniMessageOverride
    {
    }

    public sealed class OmniMessageOverrideJsonConverter : JsonConverter<IOmniMessageOverride>
    {
        private readonly Dictionary<string, Type> _propNameToTypeMap = new()
        {
            { "text_message", typeof(TextMessage) },
            { "media_message", typeof(MediaMessage) },
            { "choice_message", typeof(ChoiceMessage) },
            { "card_message", typeof(CardMessage) },
            { "carousel_message", typeof(CarouselMessage) },
            { "location_message", typeof(LocationMessage) },
            { "contact_info_message", typeof(ContactInfoMessage) },
            { "list_message", typeof(ListMessage) },
            { "template_reference", typeof(OmniTemplateReference) },
        };

        public override IOmniMessageOverride? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);

            foreach (var entry in _propNameToTypeMap)
            {
                if (elem.TryGetProperty(entry.Key, out var value))
                {
                    return value.Deserialize(entry.Value, options) as IOmniMessageOverride;
                }
            }

            throw new JsonException(
                $"Failed to match {nameof(IOmniMessageOverride)}, got json element: {elem.ToString()}");
        }

        public override void Write(Utf8JsonWriter writer, IOmniMessageOverride value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            var matchingType = _propNameToTypeMap.FirstOrDefault(x => x.Value == type);
            if (matchingType.Key is null)
            {
                throw new InvalidOperationException(
                    $"Value is not in range of expected types - actual type is {type.FullName}");
            }


            JsonSerializer.Serialize(writer, new Dictionary<string, object>
            {
                // dynamically cast IOmniMessageTemplate to specific type, e.g. TextMessage so avoid recursive infinite write
                { matchingType.Key, Convert.ChangeType(value, type) }
            }, options);
        }
    }
}
