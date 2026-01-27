using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public sealed class ContactMessage
    {
        [JsonConstructor]
        private ContactMessage() { }

        public ContactMessage(ChoiceResponseMessage choiceResponseMessage) =>
            ChoiceResponseMessage = choiceResponseMessage;

        public ContactMessage(FallbackMessage fallbackMessage) => FallbackMessage = fallbackMessage;

        public ContactMessage(LocationMessage locationMessage) => LocationMessage = locationMessage;

        public ContactMessage(MediaCardMessage mediaCardMessage) => MediaCardMessage = mediaCardMessage;

        public ContactMessage(MediaMessage mediaMessage) => MediaMessage = mediaMessage;

        public ContactMessage(TextMessage textMessage) => TextMessage = textMessage;

        public ContactMessage(ProductResponseMessage productResponseMessage) =>
            ProductResponseMessage = productResponseMessage;

        public ContactMessage(ChannelSpecificContactMessage channelSpecificMessage) =>
            ChannelSpecificMessage = channelSpecificMessage;

        /// <summary>
        ///     Gets or Sets ChoiceResponseMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("choice_response_message")]
        public ChoiceResponseMessage? ChoiceResponseMessage { get; init; }


        /// <summary>
        ///     Gets or Sets FallbackMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("fallback_message")]
        public FallbackMessage? FallbackMessage { get; init; }


        /// <summary>
        ///     Gets or Sets LocationMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("location_message")]
        public LocationMessage? LocationMessage { get; init; }


        /// <summary>
        ///     Gets or Sets MediaCardMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("media_card_message")]
        public MediaCardMessage? MediaCardMessage { get; init; }


        /// <summary>
        ///     Gets or Sets MediaMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("media_message")]
        public MediaMessage? MediaMessage { get; init; }


        /// <summary>
        ///     Gets or Sets ReplyTo
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("reply_to")]
        public ReplyTo? ReplyTo { get; set; }


        /// <summary>
        ///     Gets or Sets TextMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("text_message")]
        public TextMessage? TextMessage { get; init; }

        /// <summary>
        ///     Gets or Sets ProductResponseMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("product_response_message")]
        public ProductResponseMessage? ProductResponseMessage { get; init; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("channel_specific_message")]
        public ChannelSpecificContactMessage? ChannelSpecificMessage { get; init; }

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
            sb.Append("  ChannelSpecificMessage: ").Append(ChannelSpecificMessage).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
