using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Hooks.Models
{
    /// <summary>
    ///     CapabilityNotification
    /// </summary>
    public sealed class CapabilityNotification
    {
        /// <summary>
        ///     Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public ConversationChannel Channel { get; set; }


        /// <summary>
        ///     Status indicating the recipient&#39;s capability on the channel.
        /// </summary>
        [JsonPropertyName("capability_status")]
        public CapabilityStatus CapabilityStatus { get; set; }

        /// <summary>
        ///     ID generated when submitting the capability request. Can be used to detect duplicates.
        /// </summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }


        /// <summary>
        ///     The ID of the contact.
        /// </summary>
        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }


        /// <summary>
        ///     The channel identity. For example, a phone number for SMS, WhatsApp, and Viber Business.
        /// </summary>
        [JsonPropertyName("identity")]
        public string Identity { get; set; }


        /// <summary>
        ///     When capability_status is set to CAPABILITY_PARTIAL, this field includes a list of the supported channel-specific capabilities reported by the channel.
        /// </summary>
        [JsonPropertyName("channel_capabilities")]
        public List<string> ChannelCapabilities { get; set; }


        /// <summary>
        ///     Gets or Sets Reason
        /// </summary>
        [JsonPropertyName("reason")]
        public Reason Reason { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(CapabilityNotification)} {{\n");
            sb.Append($"  {nameof(RequestId)}: ").Append(RequestId).Append('\n');
            sb.Append($"  {nameof(ContactId)}: ").Append(ContactId).Append('\n');
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(Identity)}: ").Append(Identity).Append('\n');
            sb.Append($"  {nameof(CapabilityStatus)}: ").Append(CapabilityStatus).Append('\n');
            sb.Append($"  {nameof(ChannelCapabilities)}: ").Append(ChannelCapabilities).Append('\n');
            sb.Append($"  {nameof(Reason)}: ").Append(Reason).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
