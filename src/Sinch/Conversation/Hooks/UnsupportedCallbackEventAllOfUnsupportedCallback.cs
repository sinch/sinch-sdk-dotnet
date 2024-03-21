using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;

namespace Sinch.Conversation.Hooks
{
    /// <summary>
    ///     UnsupportedCallbackEventAllOfUnsupportedCallback
    /// </summary>
    public sealed class UnsupportedCallbackEventAllOfUnsupportedCallback
    {
        /// <summary>
        /// Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public ConversationChannel? Channel { get; set; }


        /// <summary>
        /// Gets or Sets ProcessingMode
        /// </summary>
        [JsonPropertyName("processing_mode")]
        public ProcessingMode? ProcessingMode { get; set; }

        /// <summary>
        ///     Normally a JSON payload as sent by the channel.
        /// </summary>
        [JsonPropertyName("payload")]
        public string Payload { get; set; }


        /// <summary>
        ///     The message ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }


        /// <summary>
        ///     The ID of the contact. This field is blank if not supported.
        /// </summary>
        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }


        /// <summary>
        ///     The ID of the conversation this message is part of. This field is blank if not supported.
        /// </summary>
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }


        /// <summary>
        ///     Gets or Sets ChannelIdentity
        /// </summary>
        [JsonPropertyName("channel_identity")]
        public ChannelIdentity ChannelIdentity { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(UnsupportedCallbackEventAllOfUnsupportedCallback)} {{\n");
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(Payload)}: ").Append(Payload).Append('\n');
            sb.Append($"  {nameof(ProcessingMode)}: ").Append(ProcessingMode).Append('\n');
            sb.Append($"  {nameof(Id)}: ").Append(Id).Append('\n');
            sb.Append($"  {nameof(ContactId)}: ").Append(ContactId).Append('\n');
            sb.Append($"  {nameof(ConversationId)}: ").Append(ConversationId).Append('\n');
            sb.Append($"  {nameof(ChannelIdentity)}: ").Append(ChannelIdentity).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
