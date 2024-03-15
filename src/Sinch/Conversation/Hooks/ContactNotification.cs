using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Contacts;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     ContactNotification
    /// </summary>
    public sealed class ContactNotification
    {
        /// <summary>
        ///     Gets or Sets Contact
        /// </summary>
        [JsonPropertyName("contact")]
        public Contact Contact { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ContactNotification)} {{\n");
            sb.Append($"  {nameof(Contact)}: ").Append(Contact).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
