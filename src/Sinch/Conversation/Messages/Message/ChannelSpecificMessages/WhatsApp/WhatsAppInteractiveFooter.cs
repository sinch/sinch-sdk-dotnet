using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message.ChannelSpecificMessages.WhatsApp
{
    /// <summary>
    ///     Footer of the interactive message.
    /// </summary>
    public sealed class WhatsAppInteractiveFooter
    {
        /// <summary>
        ///     The footer content (60 characters maximum). Emojis, Markdown and links are supported.
        /// </summary>
        [JsonPropertyName("text")]

        public required string Text { get; set; }



        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(WhatsAppInteractiveFooter)} {{\n");
            sb.Append($"  {nameof(Text)}: ").Append(Text).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }


}
