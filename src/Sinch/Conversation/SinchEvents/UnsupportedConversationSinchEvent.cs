using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.SinchEvents.Models;

namespace Sinch.Conversation.SinchEvents
{
    /// <summary>
    ///     Some of the sinch events received from the underlying channels might be specific to a single channel or may not have a proper mapping in Conversation API yet.
    /// </summary>
    public sealed class UnsupportedConversationSinchEvent : ConversationSinchEventBase
    {
        /// <summary>
        ///     Gets or Sets UnsupportedCallback
        /// </summary>
        [JsonPropertyName("unsupported_callback")]
        public UnsupportedCallback? UnsupportedCallback { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(UnsupportedConversationSinchEvent)} {{\n");
            sb.Append($"  {nameof(AppId)}: ").Append(AppId).Append('\n');
            sb.Append($"  {nameof(AcceptedTime)}: ").Append(AcceptedTime).Append('\n');
            sb.Append($"  {nameof(EventTime)}: ").Append(EventTime).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(MessageMetadata)}: ").Append(MessageMetadata).Append('\n');
            sb.Append($"  {nameof(CorrelationId)}: ").Append(CorrelationId).Append('\n');
            sb.Append($"  {nameof(UnsupportedCallback)}: ").Append(UnsupportedCallback).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
