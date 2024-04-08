using System.Text;
using System.Text.Json.Nodes;
using Sinch.Conversation.Messages;

namespace Sinch.Conversation.Conversations.Create
{
    public class CreateConversationRequest
    {
        /// <summary>
        ///     Gets or Sets ActiveChannel
        /// </summary>
        public ConversationChannel ActiveChannel { get; set; }

        /// <summary>
        ///     Flag for whether this conversation is active.
        /// </summary>
        public bool Active { get; set; }


        /// <summary>
        ///     The ID of the participating app.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; }
#endif


        /// <summary>
        ///     The ID of the participating contact.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string ContactId { get; set; }
#else
        public string ContactId { get; set; }
#endif


        /// <summary>
        ///     Arbitrary data set by the Conversation API clients. Up to 1024 characters long.
        /// </summary>
        public string Metadata { get; set; }


        /// <summary>
        ///     Arbitrary data set by the Conversation API clients and/or provided in the &#x60;conversation_metadata&#x60; field
        ///     of a SendMessageRequest. A valid JSON object.
        /// </summary>
        public JsonObject MetadataJson { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CreateConversationRequest {\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("  ActiveChannel: ").Append(ActiveChannel).Append("\n");
            sb.Append("  AppId: ").Append(AppId).Append("\n");
            sb.Append("  ContactId: ").Append(ContactId).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("  MetadataJson: ").Append(MetadataJson).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
