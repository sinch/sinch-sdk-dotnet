using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     This callback is sent when a conversation between the subscribed app and a contact is deleted.
    /// </summary>
    public sealed class ConversationDeleteEvent : CallbackEventBase
    {
        /// <summary>
        ///     Gets or Sets ConversationDeleteNotification
        /// </summary>
        [JsonPropertyName("conversation_delete_notification")]
        public ConversationNotification ConversationDeleteNotification { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ConversationDeleteEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(ConversationDeleteNotification)}: ").Append(ConversationDeleteNotification)
                .Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
