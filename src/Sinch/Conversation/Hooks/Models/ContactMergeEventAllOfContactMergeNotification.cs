using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Contacts;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     ContactMergeEventAllOfContactMergeNotification
    /// </summary>
    public sealed class ContactMergeEventAllOfContactMergeNotification
    {
        /// <summary>
        ///     Gets or Sets PreservedContact
        /// </summary>
        [JsonPropertyName("preserved_contact")]
        public Contact PreservedContact { get; set; }


        /// <summary>
        ///     Gets or Sets DeletedContact
        /// </summary>
        [JsonPropertyName("deleted_contact")]
        public Contact DeletedContact { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ContactMergeEventAllOfContactMergeNotification)} {{\n");
            sb.Append($"  {nameof(PreservedContact)}: ").Append(PreservedContact).Append('\n');
            sb.Append($"  {nameof(DeletedContact)}: ").Append(DeletedContact).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
