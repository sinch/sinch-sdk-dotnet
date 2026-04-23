using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.SinchEvents.Models;

namespace Sinch.Conversation.SinchEvents
{
    /// <summary>
    ///     This sinch event notifies the API clients about status changes of already sent app message.
    /// </summary>
    public sealed class MessageDeliveryReceiptEvent : ConversationSinchEventBase
    {
        /// <summary>
        ///     Gets or Sets MessageDeliveryReport
        /// </summary>
        [JsonPropertyName("message_delivery_report")]
        public MessageDeliveryReport? MessageDeliveryReport { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MessageDeliveryReceiptEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(MessageDeliveryReport)}: ").Append(MessageDeliveryReport).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
