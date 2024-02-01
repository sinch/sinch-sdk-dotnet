using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Messages;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Conversations.InjectMessage
{
    /// <summary>
    ///     A message on a particular channel.
    /// </summary>
    public sealed class InjectMessageRequest
    {
        [JsonIgnore]
        public string ConversationId { get; set; }

        /// <summary>
        ///     Gets or Sets Direction
        /// </summary>
        public ConversationDirection Direction { get; set; }

        /// <summary>
        ///     The processed time of the message in UTC timezone. Must be less than current_time and greater than (current_time -
        ///     30 days)
        /// </summary>
#if NET7_0_OR_GREATER
        public required DateTime AcceptTime { get; set; }
#else
        public DateTime AcceptTime { get; set; }
#endif

        /// <summary>
        ///     Gets or Sets AppMessage
        /// </summary>
        public AppMessage AppMessage { get; set; }


        /// <summary>
        ///     Gets or Sets ChannelIdentity
        /// </summary>
        public ChannelIdentity ChannelIdentity { get; set; }


        /// <summary>
        ///     The ID of the contact registered in the conversation provided.
        /// </summary>
        public string ContactId { get; set; }


        /// <summary>
        ///     Gets or Sets ContactMessage
        /// </summary>
        public ContactMessage ContactMessage { get; set; }


        /// <summary>
        ///     Optional. Metadata associated with the contact. Up to 1024 characters long.
        /// </summary>
        public string Metadata { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConversationMessageInjected {\n");
            sb.Append("  AcceptTime: ").Append(AcceptTime).Append("\n");
            sb.Append("  AppMessage: ").Append(AppMessage).Append("\n");
            sb.Append("  ChannelIdentity: ").Append(ChannelIdentity).Append("\n");
            sb.Append("  ContactId: ").Append(ContactId).Append("\n");
            sb.Append("  ContactMessage: ").Append(ContactMessage).Append("\n");
            sb.Append("  Direction: ").Append(Direction).Append("\n");
            sb.Append("  Metadata: ").Append(Metadata).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
