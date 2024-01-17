using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sinch.Conversation.Messages;
using Sinch.Core;

namespace Sinch.Conversation.Contacts
{
    public sealed class Contact
    {
        /// <summary>
        ///     Tracks the fields which where initialized. 
        /// </summary>
        private readonly ISet<string> _setFields = new HashSet<string>();

        private List<ChannelIdentity> _channelIdentities;
        private List<ConversationChannel> _channelPriority;
        private string _displayName;
        private string _email;
        private string _externalId;
        private string _id;
        private string _language;
        private string _metadata;

        /// <summary>
        ///     List of channel identities.
        /// </summary>
        public List<ChannelIdentity> ChannelIdentities
        {
            get => _channelIdentities;
            set
            {
                _setFields.Add(nameof(ChannelIdentities));
                _channelIdentities = value;
            }
        }


        /// <summary>
        ///     List of channels defining the channel priority.
        /// </summary>
        public List<ConversationChannel> ChannelPriority
        {
            get => _channelPriority;
            set
            {
                _setFields.Add(nameof(ChannelPriority));
                _channelPriority = value;
            }
        }


        /// <summary>
        ///     The display name. A default &#39;Unknown&#39; will be assigned if left empty.
        /// </summary>
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _setFields.Add(nameof(DisplayName));
                _displayName = value;
            }
        }


        /// <summary>
        ///     Email of the contact.
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                _setFields.Add(nameof(Email));
                _email = value;
            }
        }


        /// <summary>
        ///     Contact identifier in an external system.
        /// </summary>
        public string ExternalId
        {
            get => _externalId;
            set
            {
                _setFields.Add(nameof(ExternalId));
                _externalId = value;
            }
        }


        /// <summary>
        ///     The ID of the contact.
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                _setFields.Add(nameof(Id));
                _id = value;
            }
        }


        /// <summary>
        ///     Gets or Sets Language
        /// </summary>
        public string Language
        {
            get => _language;
            set
            {
                _setFields.Add(nameof(Language));
                _language = value;
            }
        }


        /// <summary>
        ///     Metadata associated with the contact. Up to 1024 characters long.
        /// </summary>
        public string Metadata
        {
            get => _metadata;
            set
            {
                _setFields.Add(nameof(Metadata));
                _metadata = value;
            }
        }

        /// <summary>
        ///     Get the comma separated snake_case list of properties which were directly initialized in this object.
        ///     If, for example, DisplayName and Metadata were set, will return <example>display_name,metadata</example>
        /// </summary>
        /// <returns></returns>
        internal string GetPropertiesMask()
        {
            return string.Join(',', _setFields.Select(StringUtils.ToSnakeCase));
        }

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
