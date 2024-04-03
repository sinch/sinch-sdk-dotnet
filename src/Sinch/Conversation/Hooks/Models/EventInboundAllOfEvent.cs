using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     EventInboundAllOfEvent
    /// </summary>
    public sealed class EventInboundAllOfEvent
    {
        /// <summary>
        ///     The direction of the event. It&#39;s always TO_APP for contact events.
        /// </summary>
        [JsonPropertyName("direction")]
        public ConversationDirection? Direction { get; set; }


        /// <summary>
        ///     Gets or Sets ProcessingMode
        /// </summary>
        [JsonPropertyName("processing_mode")]
        public ProcessingMode? ProcessingMode { get; set; }

        /// <summary>
        ///     The event ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }


        /// <summary>
        ///     Gets or Sets ContactEvent
        /// </summary>
        [JsonPropertyName("contact_event")]
        public ContactEvent ContactEvent { get; set; }


        /// <summary>
        ///     Gets or Sets ContactMessageEvent
        /// </summary>
        [JsonPropertyName("contact_message_event")]
        public ContactMessageEvent ContactMessageEvent { get; set; }


        /// <summary>
        ///     Gets or Sets ChannelIdentity
        /// </summary>
        [JsonPropertyName("channel_identity")]
        public ChannelIdentity ChannelIdentity { get; set; }


        /// <summary>
        ///     The ID of the contact. Will be empty if processing_mode is DISPATCH.
        /// </summary>
        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }


        /// <summary>
        ///     The ID of the conversation this event is part of. Will be empty if processing_mode is DISPATCH.
        /// </summary>
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }


        /// <summary>
        ///     Timestamp marking when the channel callback was received by the Conversation API.
        /// </summary>
        [JsonPropertyName("accept_time")]
        public DateTime AcceptTime { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(EventInboundAllOfEvent)} {{\n");
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(Direction)}: ").Append(Direction).Append('\n');
            sb.Append($"  {nameof(ContactEvent)}: ").Append(ContactEvent).Append('\n');
            sb.Append($"  {nameof(ContactMessageEvent)}: ").Append(ContactMessageEvent).Append('\n');
            sb.Append($"  {nameof(ChannelIdentity)}: ").Append(ChannelIdentity).Append('\n');
            sb.Append($"  {nameof(ContactId)}: ").Append(ContactId).Append('\n');
            sb.Append($"  {nameof(ConversationId)}: ").Append(ConversationId).Append('\n');
            sb.Append($"  {nameof(AcceptTime)}: ").Append(AcceptTime).Append('\n');
            sb.Append($"  {nameof(ProcessingMode)}: ").Append(ProcessingMode).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
