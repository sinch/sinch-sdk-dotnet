using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    /// MMS message body with media attachments
    /// </summary>
    public class MediaMessageBody
    {
        /// <summary>
        /// Text message content
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// MMS subject line
        /// </summary>
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }

        /// <summary>
        /// List of media items attached to the message
        /// </summary>
        [JsonPropertyName("media")]
        public List<MediaMessageBodyDetails>? Media { get; set; }
    }
}
