using System.Text;

namespace Sinch.Conversation.TemplatesV2
{
    public sealed class TypeTemplateVariable
    {
        /// <summary>
        ///     Gets or Sets Key
        /// </summary>
        public string? Key { get; set; }


        /// <summary>
        ///     Gets or Sets PreviewValue
        /// </summary>
        public string? PreviewValue { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TypeTemplateVariable {\n");
            sb.Append("  Key: ").Append(Key).Append("\n");
            sb.Append("  PreviewValue: ").Append(PreviewValue).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
