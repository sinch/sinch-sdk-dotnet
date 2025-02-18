using System.Collections.Generic;
using System.Text;
using Sinch.Conversation.Common;

namespace Sinch.Conversation.Contacts.Create
{
    public sealed class CreateContactRequest
    {
        /// <summary>
        ///     List of channel identities. Array must contain at least one item.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<ChannelIdentity> ChannelIdentities { get; set; }
#else
        public List<ChannelIdentity> ChannelIdentities { get; set; } = null!;
#endif

        /// <summary>
        ///     Gets or Sets Language
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Language { get; set; }
#else
        public string Language { get; set; } = null!;
#endif

        /// <summary>
        ///     List of channels defining the channel priority. The channel at the top of the list is tried first.
        /// </summary>
        public List<ConversationChannel>? ChannelPriority { get; set; }


        /// <summary>
        ///     The display name. A default &#39;Unknown&#39; will be assigned if left empty.
        /// </summary>
        public string? DisplayName { get; set; }


        /// <summary>
        ///     Email of the contact.
        /// </summary>
        public string? Email { get; set; }


        /// <summary>
        ///     Contact identifier in an external system.
        /// </summary>
        public string? ExternalId { get; set; }


        /// <summary>
        ///     Metadata associated with the contact. Up to 1024 characters long.
        /// </summary>
        public string? Metadata { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ContactCreateRequest {\n");
            sb.Append("  ChannelIdentities: ").Append(ChannelIdentities).Append("\n");
            sb.Append("  ChannelPriority: ").Append(ChannelPriority).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  ExternalId: ").Append(ExternalId).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
