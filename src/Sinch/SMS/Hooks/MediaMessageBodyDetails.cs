using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    /// Media attachment in a MMS message
    /// </summary>
    public class MediaMessageBodyDetails
    {
        /// <summary>
        /// Result code
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// MIME type of the media
        /// </summary>
        [JsonPropertyName("content_type")]
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Upload status
        /// </summary>
        [JsonPropertyName("status")]
        public MediaStatus Status { get; set; }

        /// <summary>
        /// URL to download the media file
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}

