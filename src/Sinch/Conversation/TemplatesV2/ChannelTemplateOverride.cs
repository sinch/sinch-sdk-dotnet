using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.TemplatesV2
{
    /// <summary>
    ///     Optional field to override the omnichannel template by referring to a channel-specific template.
    /// </summary>
    public sealed class ChannelTemplateOverride
    {
        /// <summary>
        ///     Gets or Sets WHATSAPP
        /// </summary>
        [JsonPropertyName("WHATSAPP")]
        public OverrideTemplateReference WHATSAPP { get; set; }
        

        /// <summary>
        ///     Gets or Sets KAKOTALK
        /// </summary>
        [JsonPropertyName("KAKOTALK")]
        public OverrideTemplateReference KAKOTALK { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ChannelTemplateOverride {\n");
            sb.Append("  WHATSAPP: ").Append(WHATSAPP).Append("\n");
            sb.Append("  KAKOTALK: ").Append(KAKOTALK).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
