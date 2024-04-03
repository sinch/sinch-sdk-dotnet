using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Hooks.Models;

namespace Sinch.Conversation.Hooks
{
   /// <summary>
    ///     This callback is used to deliver opt-in notifications from the channels.
    /// </summary>
    public sealed class OptInEvent : CallbackEventBase
    {
        /// <summary>
        ///     Gets or Sets OptInNotification
        /// </summary>
        [JsonPropertyName("opt_in_notification")]
        public OptInEventAllOfOptInNotification OptInNotification { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OptInEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(OptInNotification)}: ").Append(OptInNotification).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
