using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Common;

namespace Sinch.Conversation.Hooks.Models
{
     /// <summary>
    ///     OptInEventAllOfOptInNotification
    /// </summary>
    public sealed class OptInEventAllOfOptInNotification
    {

        /// <summary>
        /// Gets or Sets Channel
        /// </summary>
        [JsonPropertyName("channel")]
        public ConversationChannel Channel { get; set; }


        /// <summary>
        /// Status of the opt-in registration.
        /// </summary>
        [JsonPropertyName("status")]
        public OptInStatus Status { get; set; }


        /// <summary>
        /// Gets or Sets ProcessingMode
        /// </summary>
        [JsonPropertyName("processing_mode")]
        public ProcessingMode ProcessingMode { get; set; }

        /// <summary>
        ///     ID generated when making an opt-in registration request. Can be used to detect duplicates.
        /// </summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
        

        /// <summary>
        ///     The ID of the contact which is the subject of the opt-in. Will be empty if processing_mode is DISPATCH.
        /// </summary>
        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }
        

        /// <summary>
        ///     The channel identity. For example, a phone number for SMS, WhatsApp and Viber Business.
        /// </summary>
        [JsonPropertyName("identity")]
        public string Identity { get; set; }
        

        /// <summary>
        ///     Gets or Sets ErrorDetails
        /// </summary>
        [JsonPropertyName("error_details")]
        public OptInEventAllOfOptInNotificationErrorDetails ErrorDetails { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OptInEventAllOfOptInNotification)} {{\n");
            sb.Append($"  {nameof(RequestId)}: ").Append(RequestId).Append('\n');
            sb.Append($"  {nameof(ContactId)}: ").Append(ContactId).Append('\n');
            sb.Append($"  {nameof(Channel)}: ").Append(Channel).Append('\n');
            sb.Append($"  {nameof(Identity)}: ").Append(Identity).Append('\n');
            sb.Append($"  {nameof(Status)}: ").Append(Status).Append('\n');
            sb.Append($"  {nameof(ErrorDetails)}: ").Append(ErrorDetails).Append('\n');
            sb.Append($"  {nameof(ProcessingMode)}: ").Append(ProcessingMode).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
