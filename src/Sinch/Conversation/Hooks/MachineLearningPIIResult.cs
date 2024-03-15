using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     An object that contains the PII analysis of the corresponding messages.
    /// </summary>
    public sealed class MachineLearningPIIResult
    {
        /// <summary>
        ///     The message text that was analyzed.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
        

        /// <summary>
        ///     The redacted message text in which sensitive information was replaced with appropriate masks. A MISC mask is applied to a term that has been identified as PII, but with low confidence regarding which type of mask to assign.
        /// </summary>
        [JsonPropertyName("masked")]
        public string Masked { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MachineLearningPIIResult)} {{\n");
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append($"  {nameof(Masked)}: ").Append(Masked).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
