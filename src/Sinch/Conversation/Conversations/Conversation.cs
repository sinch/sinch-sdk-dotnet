using System;
using System.Text;
using System.Text.Json.Nodes;
using Sinch.Conversation.Messages;

namespace Sinch.Conversation.Conversations
{
    /// <summary>
    ///     A collection of messages exchanged between a contact and an app. Conversations are normally created on the fly by Conversation API once a message is sent and there is no active conversation already. There can be only one active conversation at any given time between a particular contact and an app.
    /// </summary>
    public sealed class Conversation
    {

        /// <summary>
        /// Gets or Sets ActiveChannel
        /// </summary>
        public ConversationChannel ActiveChannel { get; set; }

        /// <summary>
        ///     Flag for whether this conversation is active.
        /// </summary>
        public bool? Active { get; set; }
        

        /// <summary>
        ///     The ID of the participating app.
        /// </summary>
        public string AppId { get; set; }
        

        /// <summary>
        ///     The ID of the participating contact.
        /// </summary>
        public string ContactId { get; set; }
        

        /// <summary>
        ///     The ID of the conversation.
        /// </summary>
        public string Id { get; set; }
        

        /// <summary>
        ///     The timestamp of the latest message in the conversation. The timestamp will be Thursday January 01, 1970 00:00:00 UTC if the conversation contains no messages.
        /// </summary>
        public DateTime LastReceived { get; set; }
        

        /// <summary>
        ///     Arbitrary data set by the Conversation API clients. Up to 1024 characters long.
        /// </summary>
        public string Metadata { get; set; }
        

        /// <summary>
        ///     Arbitrary data set by the Conversation API clients and/or provided in the &#x60;conversation_metadata&#x60; field of a SendMessageRequest. A valid JSON object.
        /// </summary>
        public JsonObject MetadataJson { get; set; }

        public string CorrelationId { get; set; }
        
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
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
