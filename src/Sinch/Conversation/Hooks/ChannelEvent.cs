using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     This callback is used to deliver notifications regarding channel-specific information and updates. For example, if your are using the WhatsApp channel of the Conversation API, and your quality rating has been changed to GREEN, a POST would be made to the CHANNEL_EVENT webhook.
    /// </summary>
    public sealed class ChannelEvent : CallbackEventBase
    {
        /// <summary>
        ///     Gets or Sets ChannelEventNotification
        /// </summary>
        [JsonPropertyName("channel_event_notification")]
        public EventNotification EventNotification { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ChannelEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(EventNotification)}: ").Append(EventNotification).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
