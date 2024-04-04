using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;

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

        /// <summary>
        ///     Optional. Channel specific messages, overriding any transcoding.
        ///     The key in the map must point to a valid conversation channel as defined by the enum ConversationChannel.
        /// </summary>
        public Dictionary<ConversationChannel, JsonValue> ExplicitChannelMessage { get; set; }

        /// <inheritdoc cref="Agent" />        
        public Agent Agent { get; set; }

        /// <summary>
        ///     Gets or Sets AdditionalProperties
        /// </summary>
        public AppMessageAdditionalProperties AdditionalProperties { get; set; }

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
    }

    /// <summary>
    ///     Additional properties of the message.
    /// </summary>
    public sealed class AppMessageAdditionalProperties
    {
        /// <summary>
        ///     The &#x60;display_name&#x60; of the newly created contact in case it doesn&#39;t exist.
        /// </summary>
        public string ContactName { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AppMessageAdditionalProperties {\n");
            sb.Append("  ContactName: ").Append(ContactName).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
