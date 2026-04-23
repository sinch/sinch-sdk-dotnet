using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.SinchEvents.Models;

namespace Sinch.Conversation.SinchEvents
{
    /// <summary>
    ///     This sinch event delivers contact (end-user) messages to the API clients.
    /// </summary>
    public sealed class MessageInboundEvent : ConversationSinchEventBase
    {
        /// <summary>
        ///     Gets or Sets Message
        /// </summary>
        [JsonPropertyName("message")]
        public MessageInboundEventItem? Message { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MessageInboundEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
