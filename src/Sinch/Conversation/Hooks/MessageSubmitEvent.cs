using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     This callback provides a notification to the API clients that the corresponding app message was submitted to a channel. This notification is created before any confirmation from Delivery Receipts.
    /// </summary>
    public sealed class MessageSubmitEvent : CallbackEventBase
    {
        /// <summary>
        ///     Gets or Sets MessageSubmitNotification
        /// </summary>
        [JsonPropertyName("message_submit_notification")]
        public MessageSubmitNotification? MessageSubmitNotification { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MessageSubmitEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(MessageSubmitNotification)}: ").Append(MessageSubmitNotification).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
