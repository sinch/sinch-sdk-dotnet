using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     DuplicatedIdentitiesEventDuplicatedIdentitiesInner
    /// </summary>
    public sealed class DuplicatedIdentitiesEventDuplicatedIdentitiesInner
    {

        /// <summary>
        /// Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public ConversationChannel Channel { get; set; }

        /// <summary>
        ///     List of duplicated ids in the specified channel.
        /// </summary>
        [JsonPropertyName("contact_ids")]
        public List<string> ContactIds { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(DuplicatedIdentitiesEventDuplicatedIdentitiesInner)} {{\n");
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(ContactIds)}: ").Append(ContactIds).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
