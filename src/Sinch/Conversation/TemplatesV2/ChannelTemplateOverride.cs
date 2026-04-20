using System.Collections.Generic;
using System.Text;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.TemplatesV2
{
    public sealed class ChannelTemplateOverride
    {
        /// <summary>
        ///     The referenced template can be an omnichannel template stored in Conversation API Template Store as AppMessage or it can reference external channel-specific template such as WhatsApp Business Template.
        /// </summary>
        public TemplateReference? TemplateReference { get; set; }

        /// <summary>
        ///     A mapping between omni-template variables and the channel-specific parameters.
        /// </summary>
        public Dictionary<string, string>? ParameterMappings { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TemplateReference {\n");
            sb.Append("  TemplateReference: ").Append(TemplateReference).Append("\n");
            sb.Append("  ParameterMappings: ").Append(ParameterMappings).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
