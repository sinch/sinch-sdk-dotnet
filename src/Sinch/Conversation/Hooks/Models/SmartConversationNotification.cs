using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     SmartConversationsEventAllOfSmartConversationNotification
    /// </summary>
    public sealed class SmartConversationNotification
    {
        /// <summary>
        /// Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public ConversationChannel? Channel { get; set; }

        /// <summary>
        ///     The unique ID of the contact that sent the message.
        /// </summary>
        [JsonPropertyName("contact_id")]
        public string? ContactId { get; set; }


        /// <summary>
        ///     The channel-specific identifier for the contact.
        /// </summary>
        [JsonPropertyName("channel_identity")]
        public string? ChannelIdentity { get; set; }


        /// <summary>
        ///     The unique ID of the corresponding message.
        /// </summary>
        [JsonPropertyName("message_id")]
        public string? MessageId { get; set; }


        /// <summary>
        ///     The ID of the conversation the app message is part of.
        /// </summary>
        [JsonPropertyName("conversation_id")]
        public string? ConversationId { get; set; }


        /// <summary>
        ///     Gets or Sets AnalysisResults
        /// </summary>
        [JsonPropertyName("analysis_results")]
        public AnalysisResult? AnalysisResults { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(SmartConversationNotification)} {{\n");
            sb.Append($"  {nameof(ContactId)}: ").Append(ContactId).Append('\n');
            sb.Append($"  {nameof(ChannelIdentity)}: ").Append(ChannelIdentity).Append('\n');
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(MessageId)}: ").Append(MessageId).Append('\n');
            sb.Append($"  {nameof(ConversationId)}: ").Append(ConversationId).Append('\n');
            sb.Append($"  {nameof(AnalysisResults)}: ").Append(AnalysisResults).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
