using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    public class AppMessage
    {
        // Thank you System.Text.Json -_-
        [JsonConstructor]
        [Obsolete("Needed for System.Text.Json", true)]
        public AppMessage()
        {
        }

        #region Oneof app message props and constructors

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextMessage TextMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CardMessage CardMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CarouselMessage CarouselMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChoiceMessage ChoiceMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LocationMessage LocationMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MediaMessage MediaMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TemplateMessage TemplateMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ListMessage ListMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ContactInfoMessage ContactInfoMessage { get; private set; }

        public AppMessage(ChoiceMessage choiceMessage)
        {
            ChoiceMessage = choiceMessage;
        }

        public AppMessage(LocationMessage locationMessage)
        {
            LocationMessage = locationMessage;
        }

        public AppMessage(MediaMessage mediaMessage)
        {
            MediaMessage = mediaMessage;
        }

        public AppMessage(TemplateMessage templateMessage)
        {
            TemplateMessage = templateMessage;
        }

        public AppMessage(ListMessage listMessage)
        {
            ListMessage = listMessage;
        }

        public AppMessage(TextMessage textMessage)
        {
            TextMessage = textMessage;
        }

        public AppMessage(CardMessage cardMessage)
        {
            CardMessage = cardMessage;
        }

        public AppMessage(CarouselMessage carouselMessage)
        {
            CarouselMessage = carouselMessage;
        }

        public AppMessage(ContactInfoMessage contactInfoMessage)
        {
            ContactInfoMessage = contactInfoMessage;
        }

        #endregion

        /// <summary>
        ///     Optional. Channel specific messages, overriding any transcoding.
        ///     The key in the map must point to a valid conversation channel as defined by the enum ConversationChannel.
        /// </summary>
        public Dictionary<ConversationChannel, JsonValue> ExplicitChannelMessage { get; set; }

        /// <summary>
        ///     Channel specific messages, overriding any transcoding.
        ///     The structure of this property is more well-defined than the open structure of
        ///     the explicit_channel_message property, and may be easier to use.
        ///     The key in the map must point to a valid conversation channel as defined in the enum ConversationChannel.
        /// </summary>
        public Dictionary<ConversationChannel, IChannelSpecificMessage> ChannelSpecificMessage { get; set; }

        /// <inheritdoc cref="Agent" />        
        public Agent Agent { get; set; }
    }

    /// <summary>
    ///     A message containing a channel specific message (not supported by OMNI types).
    /// </summary>
    [JsonDerivedType(typeof(FlowMessage))]
    [JsonConverter(typeof(ChannelSpecificMessageJsonInterfaceConverter))]
    public interface IChannelSpecificMessage
    {
        /// <summary>
        ///     Gets or Sets MessageType
        /// </summary>
        public MessageTypeEnum MessageType { get; }
    }

    public class ChannelSpecificMessageJsonInterfaceConverter : JsonConverter<IChannelSpecificMessage>
    {
        public override IChannelSpecificMessage Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            //not optimal but straightforward
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "message_type");
            var method = descriptor.Value.GetString();

            if (MessageTypeEnum.Flows.Value == method)
                return elem.Deserialize<FlowMessage>(options);

            throw new JsonException(
                $"Failed to match verification method object, got prop `{descriptor.Name}` with value `{method}`");
        }

        public override void Write(Utf8JsonWriter writer, IChannelSpecificMessage value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }

    public class FlowMessage : IChannelSpecificMessage
    {
        [JsonPropertyName("message_type")]
        [JsonInclude]
        public MessageTypeEnum MessageType { get; private set; } = MessageTypeEnum.Flows;

        [JsonPropertyName("message")]
        public FlowChannelSpecificMessage Message { get; set; }
    }

    /// <summary>
    /// Defines MessageType
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<MessageTypeEnum>))]
    public record MessageTypeEnum(string Value) : EnumRecord(Value)
    {
        public static readonly MessageTypeEnum Flows = new("FLOWS");
    }
}
