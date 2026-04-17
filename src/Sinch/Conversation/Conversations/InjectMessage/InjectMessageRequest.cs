using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Conversations.InjectMessage
{
    /// <summary>
    ///     A message on a particular channel.
    /// </summary>
    public sealed class InjectMessageRequest
    {
        /// <summary>
        ///     Creates a request to inject an app message (sent TO_CONTACT).
        /// </summary>
        public InjectMessageRequest(AppMessage appMessage)
        {
            AppMessage = appMessage;
        }

        /// <summary>
        ///     Creates a request to inject a contact message (sent TO_APP).
        /// </summary>
        public InjectMessageRequest(ContactMessage contactMessage)
        {
            ContactMessage = contactMessage;
        }

        /// <summary>
        ///     The ID of the conversation.
        /// </summary>
        public string? ConversationId { get; set; }

        /// <summary>
        ///     Gets or Sets Direction
        /// </summary>
        public required ConversationDirection Direction { get; set; }

        /// <summary>
        ///     The processed time of the message in UTC timezone. Must be less than current_time and greater than (current_time -
        ///     30 days)
        /// </summary>
        public required DateTime AcceptTime { get; set; }

        /// <summary>
        ///     Gets or Sets AppMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AppMessage? AppMessage { get; private set; }

        /// <summary>
        ///     Gets or Sets ChannelIdentity
        /// </summary>
        public required ChannelIdentity ChannelIdentity { get; set; }

        /// <summary>
        ///     The ID of the contact registered in the conversation provided.
        /// </summary>
        public required string ContactId { get; set; }

        /// <summary>
        ///     Gets or Sets ContactMessage
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ContactMessage? ContactMessage { get; private set; }

        /// <summary>
        ///     Optional. Metadata associated with the contact. Up to 1024 characters long.
        /// </summary>
        public string? Metadata { get; set; }

        /// <summary>
        ///     The sender ID to use for the injected message.
        /// </summary>
        public string? SenderId { get; set; }

        /// <summary>
        ///     Whether or not Conversation API should store contacts and conversations for the app.
        /// </summary>
        public ProcessingMode? ProcessingMode { get; set; }

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
            sb.Append("  SenderId: ").Append(SenderId).Append("\n");
            sb.Append("  ProcessingMode: ").Append(ProcessingMode).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
