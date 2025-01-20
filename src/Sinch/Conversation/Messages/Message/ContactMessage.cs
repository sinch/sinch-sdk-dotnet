using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public class ContactMessage
    {
        // Thank you System.Text.Json -_-
        [JsonConstructor]
        [Obsolete("Needed for System.Text.Json", true)]
        public ContactMessage()
        {
        }

        public ContactMessage(ChoiceResponseMessage choiceResponseMessage)
        {
            ChoiceResponseMessage = choiceResponseMessage;
        }

        public ContactMessage(FallbackMessage fallbackMessage)
        {
            FallbackMessage = fallbackMessage;
        }

        public ContactMessage(LocationMessage locationMessage)
        {
            LocationMessage = locationMessage;
        }

        public ContactMessage(MediaCarouselMessage mediaCardMessage)
        {
            MediaCardMessage = mediaCardMessage;
        }

        public ContactMessage(MediaMessage mediaMessage)
        {
            MediaMessage = mediaMessage;
        }

        public ContactMessage(ReplyTo replyTo)
        {
            ReplyTo = replyTo;
        }

        public ContactMessage(TextMessage textMessage)
        {
            TextMessage = textMessage;
        }

        /// <summary>
        ///     Gets or Sets ChoiceResponseMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("choice_response_message")]
        public ChoiceResponseMessage? ChoiceResponseMessage { get; internal set; }


        /// <summary>
        ///     Gets or Sets FallbackMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("fallback_message")]
        public FallbackMessage? FallbackMessage { get; internal set; }


        /// <summary>
        ///     Gets or Sets LocationMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("location_message")]
        public LocationMessage? LocationMessage { get; internal set; }


        /// <summary>
        ///     Gets or Sets MediaCardMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("media_card_message")]
        public MediaCarouselMessage? MediaCardMessage { get; internal set; }


        /// <summary>
        ///     Gets or Sets MediaMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("media_message")]
        public MediaMessage? MediaMessage { get; internal set; }


        /// <summary>
        ///     Gets or Sets ReplyTo
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("reply_to")]
        public ReplyTo? ReplyTo { get; set; }


        /// <summary>
        ///     Gets or Sets TextMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("text_message")]
        public TextMessage? TextMessage { get; internal set; }


        /// <summary>
        ///     Optional. Channel specific messages, overriding any transcoding.
        ///     The key in the map must point to a valid conversation channel as defined by the enum ConversationChannel.
        /// </summary>
        public Dictionary<ConversationChannel, JsonValue>? ExplicitChannelMessage { get; set; }
        
        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ContactMessage {\n");
            sb.Append("  ChoiceResponseMessage: ").Append(ChoiceResponseMessage).Append("\n");
            sb.Append("  FallbackMessage: ").Append(FallbackMessage).Append("\n");
            sb.Append("  LocationMessage: ").Append(LocationMessage).Append("\n");
            sb.Append("  MediaCardMessage: ").Append(MediaCardMessage).Append("\n");
            sb.Append("  MediaMessage: ").Append(MediaMessage).Append("\n");
            sb.Append("  ReplyTo: ").Append(ReplyTo).Append("\n");
            sb.Append("  TextMessage: ").Append(TextMessage).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
