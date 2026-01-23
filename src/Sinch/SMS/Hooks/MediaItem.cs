using System.Text.Json.Serialization;

namespace Sinch.SMS.Hooks
{
    /// <summary>
    /// Media attachment in an MMS message
    /// </summary>
    public class MediaItem
    {
        /// <summary>
        /// Result code: 0=success, 1=upload error, 2=bucket error, 3=key error
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// MIME type of the media (e.g., "image/jpeg")
        /// </summary>
        [JsonPropertyName("content_type")]
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Upload status
        /// </summary>
        [JsonPropertyName("status")]
        public MediaItemStatus Status { get; set; }

        /// <summary>
        /// URL to download the media file (null if upload failed)
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    /// <summary>
    /// Media item upload status
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MediaItemStatus
    {
        /// <summary>
        /// Media was successfully uploaded
        /// </summary>
        Uploaded,

        /// <summary>
        /// Media upload failed (check Code for reason)
        /// </summary>
        Failed
    }
}

