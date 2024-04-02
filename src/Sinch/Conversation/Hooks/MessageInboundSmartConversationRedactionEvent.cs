using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     This callback delivers contact (end-user) messages to the API clients. The content of the message goes through an A.I. analysis and is redacted if required.
    /// </summary>
    public sealed class MessageInboundSmartConversationRedactionEvent : CallbackEventBase
    {
        /// <summary>
        ///     Gets or Sets MessageRedaction
        /// </summary>
        [JsonPropertyName("message_redaction")]
        public MessageInboundEventItem MessageRedaction { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MessageInboundSmartConversationRedactionEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(MessageRedaction)}: ").Append(MessageRedaction).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
