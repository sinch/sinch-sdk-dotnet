using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     ChannelEventAllOfChannelEventNotification
    /// </summary>
    public sealed class EventNotification
    {
        /// <summary>
        /// Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public ConversationChannel? Channel { get; set; }

        /// <summary>
        ///     The type of event being reported.
        /// </summary>
        [JsonPropertyName("event_type")]
        public string? EventType { get; set; }


        /// <summary>
        ///     An object containing additional information regarding the event. The contents of the object depend on the channel and the event_type.
        /// </summary>
        [JsonPropertyName("additional_data")]
        public JsonObject? AdditionalData { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(EventNotification)} {{\n");
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(EventType)}: ").Append(EventType).Append('\n');
            sb.Append($"  {nameof(AdditionalData)}: ").Append(AdditionalData).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
