using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.SinchEvents.Models;

namespace Sinch.Conversation.SinchEvents
{
    /// <summary>
    ///     When using the Smart Conversations functionality, Machine Learning and Artificial Intelligence analyses are delivered through specific sinch events on the Conversation API.
    /// </summary>
    public sealed class SmartConversationsEvent : ConversationSinchEventBase
    {
        /// <summary>
        ///     Gets or Sets SmartConversationNotification
        /// </summary>
        [JsonPropertyName("smart_conversation_notification")]
        public SmartConversationNotification? SmartConversationNotification { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(SmartConversationsEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(SmartConversationNotification)}: ").Append(SmartConversationNotification).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}

