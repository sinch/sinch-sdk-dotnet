using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     If the contact message was a response to a previous App message then this field contains information about that.
    /// </summary>
    /// <param name="MessageId">The Id of the message that this is a response to</param>
    public sealed record ReplyTo(string MessageId)
    {
        /// <summary>
        ///      The Id of the message that this is a response to
        /// </summary>
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; } = MessageId;

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ReplyTo {\n");
            sb.Append("  MessageId: ").Append(MessageId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
