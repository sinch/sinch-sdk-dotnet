using System.Text;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.TemplatesV2
{
    public class OverrideTemplateReference
    {
        /// <summary>
        ///     The referenced template can be an omnichannel template stored in Conversation API Template Store as AppMessage or it can reference external channel-specific template such as WhatsApp Business Template.
        /// </summary>
        public TemplateReference TemplateReference { get; set; }
        
        /// <summary>
        ///     Gets or Sets ParameterMappings
        /// </summary>
        public TemplateReferenceParameterMappings ParameterMappings { get; set; }
        
        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TemplateReference {\n");
            sb.Append("  VarTemplateReference: ").Append(TemplateReference).Append("\n");
            sb.Append("  ParameterMappings: ").Append(ParameterMappings).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
