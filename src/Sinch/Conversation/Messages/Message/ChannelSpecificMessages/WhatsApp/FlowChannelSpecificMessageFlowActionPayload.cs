using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     FlowChannelSpecificMessageFlowActionPayload
    /// </summary>
    public sealed class FlowChannelSpecificMessageFlowActionPayload
    {
        /// <summary>
        ///     The ID of the screen displayed first. This must be an entry screen.
        /// </summary>
        [JsonPropertyName("screen")]
        public string? Screen { get; set; }


        /// <summary>
        ///     Data for the first screen.
        /// </summary>
        [JsonPropertyName("data")]
        public JsonObject? Data { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(FlowChannelSpecificMessageFlowActionPayload)} {{\n");
            sb.Append($"  {nameof(Screen)}: ").Append(Screen).Append('\n');
            sb.Append($"  {nameof(Data)}: ").Append(Data).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
