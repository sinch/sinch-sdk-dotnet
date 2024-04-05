using System;
using System.Text;
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
        public ChoiceResponseMessage ChoiceResponseMessage { get; private set; }


        /// <summary>
        ///     Gets or Sets FallbackMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public FallbackMessage FallbackMessage { get; private set; }


        /// <summary>
        ///     Gets or Sets LocationMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LocationMessage LocationMessage { get; private set; }


        /// <summary>
        ///     Gets or Sets MediaCardMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MediaCarouselMessage MediaCardMessage { get; private set; }


        /// <summary>
        ///     Gets or Sets MediaMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MediaMessage MediaMessage { get; private set; }


        /// <summary>
        ///     Gets or Sets ReplyTo
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ReplyTo ReplyTo { get; private set; }


        /// <summary>
        ///     Gets or Sets TextMessage
        /// </summary>
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TextMessage TextMessage { get; private set; }


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
