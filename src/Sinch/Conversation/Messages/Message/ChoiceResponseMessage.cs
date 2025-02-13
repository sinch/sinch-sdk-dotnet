using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Represents a response to a choice message.
    /// </summary>
    public sealed class ChoiceResponseMessage
    {
        /// <summary>
        ///     The message id containing the choice.
        /// </summary>
        [JsonPropertyName("message_id")]
        public required string MessageId { get; set; }



        /// <summary>
        ///     The postback_data defined in the selected choice.
        /// </summary>

        [JsonPropertyName("postback_data")]
        public required string PostbackData { get; set; }



        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ChoiceResponseMessage {\n");
            sb.Append("  MessageId: ").Append(MessageId).Append("\n");
            sb.Append("  PostbackData: ").Append(PostbackData).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
