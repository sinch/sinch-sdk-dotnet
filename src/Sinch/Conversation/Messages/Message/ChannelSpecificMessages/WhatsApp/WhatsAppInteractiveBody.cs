using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     Body of the interactive message.
    /// </summary>
    public sealed class WhatsAppInteractiveBody
    {
        /// <summary>
        ///     The content of the message (1024 characters maximum). Emojis and Markdown are supported.
        /// </summary>
        [JsonPropertyName("text")]
#if NET7_0_OR_GREATER
        public required string Text { get; set; }
#else
        public string Text { get; set; }
#endif


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppInteractiveBody)} {{\n");
            sb.Append($"  {nameof(Text)}: ").Append(Text).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
