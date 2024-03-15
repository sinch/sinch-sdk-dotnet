using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Apps;
using Sinch.Conversation.Messages;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     MessageSubmitEventAllOfMessageSubmitNotification
    /// </summary>
    public sealed class MessageSubmitEventAllOfMessageSubmitNotification
    {
        /// <summary>
        /// Gets or Sets ProcessingMode
        /// </summary>
        [JsonPropertyName("processing_mode")]
        public ProcessingMode? ProcessingMode { get; set; }

        /// <summary>
        ///     The ID of the app message.
        /// </summary>
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }


        /// <summary>
        ///     The ID of the conversation the app message is part of. Will be empty if processing_mode is DISPATCH.
        /// </summary>
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }


        /// <summary>
        ///     Gets or Sets ChannelIdentity
        /// </summary>
        [JsonPropertyName("channel_identity")]
        public ChannelIdentity ChannelIdentity { get; set; }


        /// <summary>
        ///     The ID of the contact. Will be empty if processing_mode is DISPATCH.
        /// </summary>
        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }


        /// <summary>
        ///     Gets or Sets SubmittedMessage
        /// </summary>
        [JsonPropertyName("submitted_message")]
        public ContactMessage SubmittedMessage { get; set; }


        /// <summary>
        ///     Metadata specified in the message_metadata field of a Send Message request, if any.
        /// </summary>
        [JsonPropertyName("metadata")]
        public string Metadata { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MessageSubmitEventAllOfMessageSubmitNotification)} {{\n");
            sb.Append($"  {nameof(MessageId)}: ").Append(MessageId).Append('\n');
            sb.Append($"  {nameof(ConversationId)}: ").Append(ConversationId).Append('\n');
            sb.Append($"  {nameof(ChannelIdentity)}: ").Append(ChannelIdentity).Append('\n');
            sb.Append($"  {nameof(ContactId)}: ").Append(ContactId).Append('\n');
            sb.Append($"  {nameof(SubmittedMessage)}: ").Append(SubmittedMessage).Append('\n');
            sb.Append($"  {nameof(Metadata)}: ").Append(Metadata).Append('\n');
            sb.Append($"  {nameof(ProcessingMode)}: ").Append(ProcessingMode).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
