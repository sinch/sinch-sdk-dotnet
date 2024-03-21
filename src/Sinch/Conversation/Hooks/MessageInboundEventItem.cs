using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     MessageInboundEventItem
    /// </summary>
    public sealed class MessageInboundEventItem
    {
        /// <summary>
        /// The direction of the message, it&#39;s always TO_APP for contact messages.
        /// </summary>
        [JsonPropertyName("direction")]
        public ConversationDirection? Direction { get; set; }


        /// <summary>
        /// Gets or Sets ProcessingMode
        /// </summary>
        [JsonPropertyName("processing_mode")]
        public ProcessingMode? ProcessingMode { get; set; }

        /// <summary>
        ///     The message ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }


        /// <summary>
        ///     Gets or Sets ContactMessage
        /// </summary>
        [JsonPropertyName("contact_message")]
        public ContactMessage ContactMessage { get; set; }


        /// <summary>
        ///     Gets or Sets ChannelIdentity
        /// </summary>
        [JsonPropertyName("channel_identity")]
        public ChannelIdentity ChannelIdentity { get; set; }


        /// <summary>
        ///     The ID of the conversation this message is part of. Will be empty if processing_mode is DISPATCH.
        /// </summary>
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }


        /// <summary>
        ///     The ID of the contact. Will be empty if processing_mode is DISPATCH.
        /// </summary>
        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }


        /// <summary>
        ///     Usually, metadata specific to the underlying channel is provided in this field. Refer to the individual channels&#39; documentation for more information (for example, SMS delivery receipts). Note that, for Choice message responses, this field is populated with the value of the message_metadata field of the corresponding Send message request.
        /// </summary>
        [JsonPropertyName("metadata")]
        public string Metadata { get; set; }


        /// <summary>
        ///     Timestamp marking when the channel callback was received by the Conversation API.
        /// </summary>
        [JsonPropertyName("accept_time")]
        public DateTime AcceptTime { get; set; }


        /// <summary>
        ///     The sender ID to which the contact sent the message, if applicable. For example, originator msisdn/short code for SMS and MMS.
        /// </summary>
        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }


        /// <summary>
        ///     Flag for whether this message was injected.
        /// </summary>
        [JsonPropertyName("injected")]
        public bool Injected { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MessageInboundEventItem)} {{\n");
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(Direction)}: ").Append(Direction).Append('\n');
            sb.Append($"  {nameof(ContactMessage)}: ").Append(ContactMessage).Append('\n');
            sb.Append($"  {nameof(ChannelIdentity)}: ").Append(ChannelIdentity).Append('\n');
            sb.Append($"  {nameof(ConversationId)}: ").Append(ConversationId).Append('\n');
            sb.Append($"  {nameof(ContactId)}: ").Append(ContactId).Append('\n');
            sb.Append($"  {nameof(Metadata)}: ").Append(Metadata).Append('\n');
            sb.Append($"  {nameof(AcceptTime)}: ").Append(AcceptTime).Append('\n');
            sb.Append($"  {nameof(SenderId)}: ").Append(SenderId).Append('\n');
            sb.Append($"  {nameof(ProcessingMode)}: ").Append(ProcessingMode).Append('\n');
            sb.Append($"  {nameof(Injected)}: ").Append(Injected).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
