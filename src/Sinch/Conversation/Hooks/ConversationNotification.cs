using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     Object containing the details of the started / stopped / deleted conversation
    /// </summary>
    public sealed class ConversationNotification
    {
        /// <summary>
        ///     Gets or Sets Conversation
        /// </summary>
        [JsonPropertyName("conversation")]
        public Conversations.Conversation Conversation { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ConversationNotification)} {{\n");
            sb.Append($"  {nameof(Conversation)}: ").Append(Conversation).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
