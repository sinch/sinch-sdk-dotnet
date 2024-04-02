using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     DuplicatedIdentitiesEvent
    /// </summary>
    public sealed class DuplicatedIdentitiesEvent
    {
        /// <summary>
        ///     Gets or Sets DuplicatedIdentities
        /// </summary>
        [JsonPropertyName("duplicated_identities")]
        public List<DuplicatedIdentitiesEventDuplicatedIdentitiesInner> DuplicatedIdentities { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(DuplicatedIdentitiesEvent)} {{\n");
            sb.Append($"  {nameof(DuplicatedIdentities)}: ").Append(DuplicatedIdentities).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
