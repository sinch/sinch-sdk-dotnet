using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    public class AppMessage
    {
        public AppMessage(IMessage message)
        {
            switch (message)
            {
                case TextMessage textMessage:
                    TextMessage = textMessage;
                    break;
                case LocationMessage locationMessage:
                    LocationMessage = locationMessage;
                    break;
                case CardMessage cardMessage:
                    CardMessage = cardMessage;
                    break;
                case CarouselMessage carouselMessage:
                    CarouselMessage = carouselMessage;
                    break;
                case ChoiceMessage choiceMessage:
                    ChoiceMessage = choiceMessage;
                    break;
                case MediaMessage mediaMessage:
                    MediaMessage = mediaMessage;
                    break;
                case TemplateMessage templateMessage:
                    TemplateMessage = templateMessage;
                    break;
                case ListMessage listMessage:
                    ListMessage = listMessage;
                    break;
            }
        }

        /// <summary>
        ///     Optional. Channel specific messages, overriding any transcoding.
        ///     The key in the map must point to a valid conversation channel as defined by the enum ConversationChannel.
        /// </summary>
        public object ExplicitChannelMessage { get; set; }

        /// <summary>
        ///     Gets or Sets AdditionalProperties
        /// </summary>
        public AppMessageAdditionalProperties AdditionalProperties { get; set; }

        /// <summary>
        /// Get a text_message property
        /// </summary>
        public TextMessage TextMessage { get; private set; }

        /// <summary>
        /// Get a LocationMessage
        /// </summary>
        public LocationMessage LocationMessage { get; private set; }

        /// <summary>
        /// Get a CardMessage
        /// </summary>
        public CardMessage CardMessage { get; private set; }

        /// <summary>
        /// Get a CarouselMessage
        /// </summary>
        public CarouselMessage CarouselMessage { get; private set; }

        /// <summary>
        /// Get a ChoiceMessage
        /// </summary>
        public ChoiceMessage ChoiceMessage { get; private set; }

        /// <summary>
        /// Get a MediaMessage
        /// </summary>
        public MediaMessage MediaMessage { get; private set; }

        /// <summary>
        /// Get a TemplateMessage
        /// </summary>
        public TemplateMessage TemplateMessage { get; private set; }

        /// <summary>
        /// Get a ListMessage
        /// </summary>
        public ListMessage ListMessage { get; private set; }
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
