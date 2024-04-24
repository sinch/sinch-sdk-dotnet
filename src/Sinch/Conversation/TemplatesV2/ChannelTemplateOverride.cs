using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.TemplatesV2
{
    /// <summary>
    ///     Optional field to override the omnichannel template by referring to a channel-specific template.
    /// </summary>
    public sealed class ChannelTemplateOverride
    {
        /// <summary>
        ///     Gets or Sets Whatsapp
        /// </summary>
        [JsonPropertyName("WHATSAPP")]
        public OverrideTemplateReference? WhatsApp { get; set; }


        /// <summary>
        ///     Gets or Sets Kakaotalk
        /// </summary>
        [JsonPropertyName("KAKAOTALK")]
        public OverrideTemplateReference? KakaoTalk { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ChannelTemplateOverride {\n");
            sb.Append("  WhatsApp: ").Append(WhatsApp).Append("\n");
            sb.Append("  KakaotTalk: ").Append(KakaoTalk).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
