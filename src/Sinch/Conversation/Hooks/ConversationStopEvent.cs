using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     This callback is sent when a conversation between the subscribed app and a contact is stopped.
    /// </summary>
    public sealed class ConversationStopEvent : CallbackEventBase
    {
        /// <summary>
        ///     Gets or Sets ConversationStopNotification
        /// </summary>
        [JsonPropertyName("conversation_stop_notification")]
        public ConversationNotification? ConversationStopNotification { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ConversationStopEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(ConversationStopNotification)}: ").Append(ConversationStopNotification).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
