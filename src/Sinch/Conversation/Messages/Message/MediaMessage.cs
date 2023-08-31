using System;
using System.Text;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     A message containing a media component, such as an image, document, or video.
    /// </summary>
    public sealed class MediaMessage : IMessage
    {
        /// <summary>
        ///     An optional parameter. Will be used where it is natively supported.
        /// </summary>
        public Uri ThumbnailUrl { get; set; }


        /// <summary>
        ///     Url to the media file.
        /// </summary>
#if NET7_0_OR_GREATER
        public required Uri Url { get; init; }
#else
        public Uri Url { get; set; }
#endif


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
