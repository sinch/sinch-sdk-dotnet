using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public class AppMessage
    {
        public AppMessage(IMessage message)
        {
            if (message is TextMessage textMessage)
            {
                TextMessage = textMessage;
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
