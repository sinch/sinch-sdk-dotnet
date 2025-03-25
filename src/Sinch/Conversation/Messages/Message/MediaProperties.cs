using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     MediaProperties
    /// </summary>
    public sealed class MediaProperties
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
        public required string Url { get; set; }
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
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MediaProperties)} {{\n");
            sb.Append($"  {nameof(ThumbnailUrl)}: ").Append(ThumbnailUrl).Append('\n');
            sb.Append($"  {nameof(Url)}: ").Append(Url).Append('\n');
            sb.Append($"  {nameof(FilenameOverride)}: ").Append(FilenameOverride).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
