using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     OffensiveAnalysis
    /// </summary>
    public sealed class OffensiveAnalysis
    {
        /// <summary>
        /// A label, either SAFE or UNSAFE, that classifies the analyzed content.
        /// </summary>
        [JsonPropertyName("evaluation")]
        public Evaluation? Evaluation { get; set; }

        /// <summary>
        ///     Either the message text or the URL of the image that was analyzed.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }


        /// <summary>
        ///     URL of the image that was analyzed.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }


        /// <summary>
        ///     The likelihood that the assigned evaluation represents the analyzed message correctly. 1 is the maximum value, representing the highest likelihood that the content of the message matches the evaluation. 0 is the minimum value, representing the lowest likelihood that the content of the message matches the evaluation.
        /// </summary>
        [JsonPropertyName("score")]
        public float? Score { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OffensiveAnalysis)} {{\n");
            sb.Append($"  {nameof(Message)}: ").Append(Message).Append('\n');
            sb.Append($"  {nameof(Url)}: ").Append(Url).Append('\n');
            sb.Append($"  {nameof(Evaluation)}: ").Append(Evaluation).Append('\n');
            sb.Append($"  {nameof(Score)}: ").Append(Score).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
