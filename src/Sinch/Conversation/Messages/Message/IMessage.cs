using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Marker interface for app messages
    /// </summary>
    [JsonInterfaceConverter(typeof(InterfaceConverter<IMessage>))]
    public interface IMessage
    {
    }

    public class MessageJsonConverter : JsonConverter<IMessage>
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
            { "template_reference", typeof(TemplateReference) },
        };

        public override IMessage? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var elem = JsonElement.ParseValue(ref reader);

            foreach (var entry in _propNameToTypeMap)
            {
                if (elem.TryGetProperty(entry.Key, out var value))
                {
                    return value.Deserialize(entry.Value, options) as IMessage;
                }
            }

            throw new JsonException(
                $"Failed to match {nameof(IMessage)}, got json element: {elem.ToString()}");
        }

        public override void Write(Utf8JsonWriter writer, IMessage value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            var matchingType = _propNameToTypeMap.FirstOrDefault(x => x.Value == type);
            if (matchingType.Key is null)
            {
                throw new InvalidOperationException(
                    $"Value is not in range of expected types - actual type is {type.FullName}");
            }


            JsonSerializer.Serialize(writer, Convert.ChangeType(value, type), options);
        }
    }
}
