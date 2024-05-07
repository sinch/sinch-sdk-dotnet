using System.Text;

namespace Sinch.Conversation.TemplatesV2
{
    /// <summary>
    ///     A mapping between omni-template variables and the channel specific parameters.
    /// </summary>
    public sealed class TemplateReferenceParameterMappings
    {
        /// <summary>
        ///     The mapping between the omni-template variable and the channel specific parameter.
        /// </summary>
        public string? Name { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TemplateReferenceParameterMappings {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
