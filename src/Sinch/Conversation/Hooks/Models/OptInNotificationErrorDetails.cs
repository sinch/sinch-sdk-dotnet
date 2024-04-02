using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     This field is populated if the opt-in failed.
    /// </summary>
    public sealed class OptInNotificationErrorDetails
    {
        /// <summary>
        ///     Human-readable error description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OptInNotificationErrorDetails)} {{\n");
            sb.Append($"  {nameof(Description)}: ").Append(Description).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
