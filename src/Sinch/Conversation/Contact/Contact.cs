using System.Collections.Generic;
using System.Text;
using Sinch.Conversation.Messages;

namespace Sinch.Conversation.Contact
{
    public class Contact
    {
        /// <summary>
        ///     List of channel identities.
        /// </summary>
        public List<ChannelIdentity> ChannelIdentities { get; set; }


        /// <summary>
        ///     List of channels defining the channel priority.
        /// </summary>
        public List<ConversationChannel> ChannelPriority { get; set; }


        /// <summary>
        ///     The display name. A default &#39;Unknown&#39; will be assigned if left empty.
        /// </summary>
        public string DisplayName { get; set; }


        /// <summary>
        ///     Email of the contact.
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        ///     Contact identifier in an external system.
        /// </summary>
        public string ExternalId { get; set; }


        /// <summary>
        ///     The ID of the contact.
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        ///     Gets or Sets Language
        /// </summary>
        public string Language { get; set; }


        /// <summary>
        ///     Metadata associated with the contact. Up to 1024 characters long.
        /// </summary>
        public string Metadata { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Contact {\n");
            sb.Append("  ChannelIdentities: ").Append(ChannelIdentities).Append("\n");
            sb.Append("  ChannelPriority: ").Append(ChannelPriority).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  ExternalId: ").Append(ExternalId).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
