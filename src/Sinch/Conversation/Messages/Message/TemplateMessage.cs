using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message

{
    /// <summary>
    ///     TemplateMessage
    /// </summary>
    public sealed class TemplateMessage
    {
        /// <summary>
        ///     Optional. Channel specific template reference with parameters per channel.
        ///     The channel template if exists overrides the omnichannel template.
        ///     At least one of &#x60;channel_template&#x60; or &#x60;omni_template&#x60; needs to be present.
        ///     The key in the map must point to a valid conversation channel as defined by the enum ConversationChannel.
        /// </summary>
        [JsonPropertyName("channel_template")]
        public Dictionary<ConversationChannel, TemplateReference>? ChannelTemplate { get; set; }


        /// <summary>
        ///     The referenced template can be an omnichannel template stored in Conversation API
        ///     Template Store as AppMessage or it can reference external channel-specific template
        ///     such as WhatsApp Business Template.
        /// </summary>
        [JsonPropertyName("omni_template")]
        public OmniTemplateReference? OmniTemplate { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class TemplateMessage {\n");
            sb.Append("  ChannelTemplate: ").Append(ChannelTemplate).Append("\n");
            sb.Append("  OmniTemplate: ").Append(OmniTemplate).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     The referenced template can be an omnichannel template stored in Conversation API Template Store
    ///     as AppMessage or it can reference external channel-specific template such as WhatsApp Business Template.
    /// </summary>
    public sealed class OmniTemplateReference : TemplateReferenceBase, IOmniMessageOverride
    {
        /// <summary>
        ///     Used to specify what version of a template to use. This will be used in conjunction with &#x60;language_code&#x60;.
        /// </summary>
        [JsonPropertyName("version")]
        public required string Version { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"class {nameof(OmniTemplateReference)} {{\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  Parameters: ").Append(Parameters).Append("\n");
            sb.Append("  TemplateId: ").Append(TemplateId).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     The referenced template can be an omnichannel template stored in Conversation API Template Store
    ///     as AppMessage or it can reference external channel-specific template such as WhatsApp Business Template.
    /// </summary>
    public sealed class TemplateReference : TemplateReferenceBase
    {
        /// <summary>
        ///     Used to specify what version of a template to use. This will be used in conjunction with &#x60;language_code&#x60;.
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"class {nameof(TemplateReference)} {{\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  Parameters: ").Append(Parameters).Append("\n");
            sb.Append("  TemplateId: ").Append(TemplateId).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     The referenced template can be an omnichannel template stored in Conversation API Template Store
    ///     as AppMessage or it can reference external channel-specific template such as WhatsApp Business Template.
    /// </summary>
    public abstract class TemplateReferenceBase
    {
        /// <summary>
        ///     The ID of the template.
        /// </summary>
        [JsonPropertyName("template_id")]
        public required string TemplateId { get; set; }

        /// <summary>
        ///     The BCP-47 language code, such as &#x60;en-US&#x60; or &#x60;sr-Latn&#x60;. For more information,
        ///     see http://www.unicode.org/reports/tr35/#Unicode_locale_identifier. English is the default language_code.
        /// </summary>
        [JsonPropertyName("language_code")]
        public string? LanguageCode { get; set; }


        /// <summary>
        ///     Required if the template has parameters.
        ///     Concrete values must be present for all defined parameters in the template.
        ///     Parameters can be different for different versions and/or languages of the template.
        /// </summary>
        [JsonPropertyName("parameters")]
        public Dictionary<string, string>? Parameters { get; set; }
    }
}
