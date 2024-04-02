using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     An object that identifies a document type within the image, along with a confidence level for that document type.
    /// </summary>
    public sealed class DocumentImageClassification
    {
        /// <summary>
        ///     The document type that the analyzed image most likely contains.
        /// </summary>
        [JsonPropertyName("doc_type")]
        public string DocType { get; set; }
        

        /// <summary>
        ///     The likelihood that the analyzed image contains the assigned document type. 1 is the maximum value, representing the highest likelihood that the analyzed image contains the assigned document type, and 0 is the minimum value, representing the lowest likelihood that the analyzed image contains the assigned document type.
        /// </summary>
        [JsonPropertyName("confidence")]
        public float Confidence { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(DocumentImageClassification)} {{\n");
            sb.Append($"  {nameof(DocType)}: ").Append(DocType).Append('\n');
            sb.Append($"  {nameof(Confidence)}: ").Append(Confidence).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
