using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Numbers.Callbacks
{
    /// <summary>
    ///     Response message containing the callbacks configuration for a specific project
    /// </summary>
    // name ref: CallbackConfiguration
    public sealed class CallbackConfiguration
    {
        /// <summary>
        ///     Gets or Sets ProjectId
        /// </summary>
        [JsonPropertyName("projectId")]
        public string? ProjectId { get; set; }


        /// <summary>
        ///     Gets or Sets HmacSecret
        /// </summary>
        [JsonPropertyName("hmacSecret")]
        public string? HmacSecret { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(CallbackConfiguration)} {{\n");
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append($"  {nameof(HmacSecret)}: ").Append(Consts.HiddenString).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
