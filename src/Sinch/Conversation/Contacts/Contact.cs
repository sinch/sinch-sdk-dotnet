using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Core;

namespace Sinch.Conversation.Contacts
{
    public sealed class Contact : PropertyMaskQuery
    {
        private List<ChannelIdentity>? _channelIdentities;
        private List<ConversationChannel>? _channelPriority;
        private string? _displayName;
        private string? _email;
        private string? _externalId;
        private string? _id;
        private ConversationLanguage? _language;
        private string? _metadata;

        /// <summary>
        ///     List of channel identities.
        /// </summary>
        [JsonPropertyName("channel_identities")]
        public List<ChannelIdentity>? ChannelIdentities
        {
            get => _channelIdentities;
            set
            {
                SetFields.Add(nameof(ChannelIdentities));
                _channelIdentities = value;
            }
        }


        /// <summary>
        ///     List of channels defining the channel priority.
        /// </summary>
        [JsonPropertyName("channel_priority")]
        public List<ConversationChannel>? ChannelPriority
        {
            get => _channelPriority;
            set
            {
                SetFields.Add(nameof(ChannelPriority));
                _channelPriority = value;
            }
        }


        /// <summary>
        ///     The display name. A default &#39;Unknown&#39; will be assigned if left empty.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string? DisplayName
        {
            get => _displayName;
            set
            {
                SetFields.Add(nameof(DisplayName));
                _displayName = value;
            }
        }


        /// <summary>
        ///     Email of the contact.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email
        {
            get => _email;
            set
            {
                SetFields.Add(nameof(Email));
                _email = value;
            }
        }


        /// <summary>
        ///     Contact identifier in an external system.
        /// </summary>
        [JsonPropertyName("external_id")]
        public string? ExternalId
        {
            get => _externalId;
            set
            {
                SetFields.Add(nameof(ExternalId));
                _externalId = value;
            }
        }


        /// <summary>
        ///     The ID of the contact.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id
        {
            get => _id;
            set
            {
                SetFields.Add(nameof(Id));
                _id = value;
            }
        }


        /// <summary>
        ///     Gets or Sets Language
        /// </summary>
        [JsonPropertyName("language")]
        public ConversationLanguage? Language
        {
            get => _language;
            set
            {
                SetFields.Add(nameof(Language));
                _language = value;
            }
        }


        /// <summary>
        ///     Metadata associated with the contact. Up to 1024 characters long.
        /// </summary>
        [JsonPropertyName("metadata")]
        public string? Metadata
        {
            get => _metadata;
            set
            {
                SetFields.Add(nameof(Metadata));
                _metadata = value;
            }
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
