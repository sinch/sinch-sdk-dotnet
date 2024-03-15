

using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     This field is populated if the opt-out failed.
    /// </summary>
    public sealed class OptOutEventAllOfOptOutNotificationErrorDetails
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
            sb.Append($"class {nameof(OptOutEventAllOfOptOutNotificationErrorDetails)} {{\n");
            sb.Append($"  {nameof(Description)}: ").Append(Description).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }

}
