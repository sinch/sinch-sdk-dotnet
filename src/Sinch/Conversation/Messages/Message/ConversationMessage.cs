using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    public class ConversationMessage
    {
             /// <summary>
        /// Gets or Sets Direction
        /// </summary>
        public ConversationDirection Direction { get; set; }

        /// <summary>
        ///     The time Conversation API processed the message.
        /// </summary>
        [JsonInclude]
        public DateTime AcceptTime { get; private set; }
        

        /// <summary>
        ///     Gets or Sets AppMessage
        /// </summary>

        public AppMessage AppMessage { get; set; }
        

        /// <summary>
        ///     Gets or Sets ChannelIdentity
        /// </summary>
        public ChannelIdentity ChannelIdentity { get; set; }
        

        /// <summary>
        ///     The ID of the contact.
        /// </summary>
        public string ContactId { get; set; }
        

        /// <summary>
        ///     Gets or Sets ContactMessage
        /// </summary>
        public ContactMessage ContactMessage { get; set; }
        

        /// <summary>
        ///     The ID of the conversation.
        /// </summary>
        public string ConversationId { get; set; }
        

        /// <summary>
        ///     The ID of the message.
        /// </summary>
        public string Id { get; set; }
        

        /// <summary>
        ///     Optional. Metadata associated with the contact. Up to 1024 characters long.
        /// </summary>
        public string Metadata { get; set; }
        

        /// <summary>
        ///     Flag for whether this message was injected.
        /// </summary>
        public bool Injected { get; private set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConversationMessage {\n");
            sb.Append("  AcceptTime: ").Append(AcceptTime).Append("\n");
            sb.Append("  AppMessage: ").Append(AppMessage).Append("\n");
            sb.Append("  ChannelIdentity: ").Append(ChannelIdentity).Append("\n");
            sb.Append("  ContactId: ").Append(ContactId).Append("\n");
            sb.Append("  ContactMessage: ").Append(ContactMessage).Append("\n");
            sb.Append("  ConversationId: ").Append(ConversationId).Append("\n");
            sb.Append("  Direction: ").Append(Direction).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("  Injected: ").Append(Injected).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
