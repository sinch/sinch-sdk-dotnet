using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp;
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
        [JsonPropertyName("text_message")]
        public TextMessage? TextMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("card_message")]
        public CardMessage? CardMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("carousel_message")]
        public CarouselMessage? CarouselMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("choice_message")]
        public ChoiceMessage? ChoiceMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("location_message")]
        public LocationMessage? LocationMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("media_message")]
        public MediaMessage? MediaMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("template_message")]
        public TemplateMessage? TemplateMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("list_message")]
        public ListMessage? ListMessage { get; private set; }

        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("contact_info_message")]
        public ContactInfoMessage? ContactInfoMessage { get; private set; }

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
        [JsonPropertyName("explicit_channel_message")]
        public Dictionary<ConversationChannel, string>? ExplicitChannelMessage { get; set; }

        /// <summary>
        ///     Channel specific messages, overriding any transcoding.
        ///     The structure of this property is more well-defined than the open structure of
        ///     the explicit_channel_message property, and may be easier to use.
        ///     The key in the map must point to a valid conversation channel as defined in the enum ConversationChannel.
        /// </summary>
        [JsonPropertyName("channel_specific_message")]
        public Dictionary<ConversationChannel, IChannelSpecificMessage>? ChannelSpecificMessage { get; set; }

        [JsonPropertyName("explicit_channel_omni_message")]
        public Dictionary<ChannelSpecificTemplate, IOmniMessageOverride>? ExplicitChannelOmniMessage { get; set; }


        /// <inheritdoc cref="Agent" />
        [JsonPropertyName("agent")]
        public Agent? Agent { get; set; }
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
        public MessageType MessageType { get; }
    }



    [JsonConverter(typeof(EnumRecordJsonConverter<ChannelSpecificTemplate>))]
    public record ChannelSpecificTemplate(string Value) : EnumRecord(Value)
    {
        public static readonly ChannelSpecificTemplate WhatsApp = new ChannelSpecificTemplate("WHATSAPP");
        public static readonly ChannelSpecificTemplate KakaoTalk = new ChannelSpecificTemplate("KAKAOTALK");
        public static readonly ChannelSpecificTemplate WeChat = new ChannelSpecificTemplate("WECHAT");
    }

    public class ChannelSpecificMessageJsonInterfaceConverter : JsonConverter<IChannelSpecificMessage>
    {
        public override IChannelSpecificMessage Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            // not optimal but straightforward
            var elem = JsonElement.ParseValue(ref reader);
            var descriptor = elem.EnumerateObject().FirstOrDefault(x => x.Name == "message_type");
            var method = descriptor.Value.GetString();

            if (MessageType.Flows.Value == method)
                return elem.Deserialize<FlowMessage>(options) ??
                       throw new InvalidOperationException($"{nameof(FlowMessage)} deserialization result is null.");

            throw new JsonException(
                $"Failed to match {nameof(IChannelSpecificMessage)}, got prop `{descriptor.Name}` with value `{method}`");
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
        public MessageType MessageType { get; private set; } = MessageType.Flows;

        [JsonPropertyName("message")]
        public FlowChannelSpecificMessage? Message { get; set; }
    }

    /// <summary>
    ///     Defines MessageType
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<MessageType>))]
    public record MessageType(string Value) : EnumRecord(Value)
    {
        public static readonly MessageType Flows = new("FLOWS");
    }
}
