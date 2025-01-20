using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     An object containing a result object that reports on all identified fields, as well as the values assigned to those fields.
    /// </summary>
    public sealed class DocumentFieldClassification
    {
        /// <summary>
        ///     The result of the Document Field Classification process
        /// </summary>
        [JsonPropertyName("result")]
        public List<Dictionary<string, DocumentFieldData>>? Result { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(DocumentFieldClassification)} {{\n");
            sb.Append($"  {nameof(Result)}: ").Append(Result).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

    public class DocumentFieldData
    {
        /// <summary>
        ///    The data array contains the string(s) assigned to the corresponding document field.
        /// </summary>
        [JsonPropertyName("data")]
        public List<string>? Data { get; set; }
    }
}
