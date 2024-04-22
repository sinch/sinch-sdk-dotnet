using System;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Conversations
{
    /// <summary>
    ///     A collection of messages exchanged between a contact and an app. Conversations are normally created on the fly by
    ///     Conversation API once a message is sent and there is no active conversation already. There can be only one active
    ///     conversation at any given time between a particular contact and an app.
    /// </summary>
    public sealed class Conversation : PropertyMaskQuery
    {
        private bool? _active;
        private ConversationChannel? _activeChannel;
        private string? _appId;
        private string? _contactId;
        private string? _correlationId;
        private string? _metadata;
        private JsonObject? _metadataJson;

        /// <summary>
        ///     Gets or Sets ActiveChannel
        /// </summary>
        public ConversationChannel? ActiveChannel
        {
            get => _activeChannel;
            set
            {
                SetFields.Add(nameof(ActiveChannel));
                _activeChannel = value;
            }
        }


        /// <summary>
        ///     Flag for whether this conversation is active.
        /// </summary>
        public bool? Active
        {
            get => _active;
            set
            {
                SetFields.Add(nameof(Active));
                _active = value;
            }
        }

        /// <summary>
        ///     The ID of the participating app.
        /// </summary>
        public string? AppId
        {
            get => _appId;
            set
            {
                SetFields.Add(nameof(AppId));
                _appId = value;
            }
        }

        /// <summary>
        ///     The ID of the participating contact.
        /// </summary>
        public string? ContactId
        {
            get => _contactId;
            set
            {
                SetFields.Add(nameof(ContactId));
                _contactId = value;
            }
        }

        /// <summary>
        ///     The ID of the conversation.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        ///     The timestamp of the latest message in the conversation.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime LastReceived { get; set; }

        /// <summary>
        ///     Arbitrary data set by the Conversation API clients. Up to 1024 characters long.
        /// </summary>
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
        ///     Arbitrary data set by the Conversation API clients and/or provided in the conversation_metadata field of a
        ///     SendMessageRequest. A valid JSON object.
        /// </summary>
        public JsonObject? MetadataJson
        {
            get => _metadataJson;
            set
            {
                SetFields.Add(nameof(MetadataJson));
                _metadataJson = value;
            }
        }

        public string? CorrelationId
        {
            get => _correlationId;
            set
            {
                SetFields.Add(nameof(CorrelationId));
                _correlationId = value;
            }
        }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Conversation {\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("  ActiveChannel: ").Append(ActiveChannel).Append("\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  ContactId: ").Append(ContactId).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  LastReceived: ").Append(LastReceived).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("  MetadataJson: ").Append(MetadataJson).Append("\n");
            sb.Append("  CorrelationId: ").Append(CorrelationId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
