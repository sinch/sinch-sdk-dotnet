using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     A message containing a media component, such as an image, document, or video.
    /// </summary>
    public sealed class MediaMessage : IOmniMessageOverride
    {
        /// <summary>
        ///     An optional parameter. Will be used where it is natively supported.
        /// </summary>
        [JsonPropertyName("thumbnail_url")]
        public string? ThumbnailUrl { get; set; }


        /// <summary>
        ///     Url to the media file.
        /// </summary>
        [JsonPropertyName("url")]
#if NET7_0_OR_GREATER
        public required string Url { get; init; }
#else
        public string Url { get; set; } = null!;
#endif

        /// <summary>
        ///     Overrides the media file name.
        /// </summary>
        [JsonPropertyName("filename_override")]
        public string? FilenameOverride { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class MediaMessage {\n");
            sb.Append("  ThumbnailUrl: ").Append(ThumbnailUrl).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
