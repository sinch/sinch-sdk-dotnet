using System.Collections.Generic;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Transcoding
{
    /// <summary>
    ///     The message to be transcoded, and the app and channels for which the message is to be transcoded.
    /// </summary>
    public sealed class TranscodeRequest
    {
#if NET7_0_OR_GREATER
        public required string AppId { get; set; }
#else
        public string AppId { get; set; } = null!;
#endif

        /// <summary>
        ///     Message originating from an app
        /// </summary>
#if NET7_0_OR_GREATER
        public required AppMessage AppMessage { get; set; }
#else
        public AppMessage AppMessage { get; set; } = null!;
#endif

        /// <summary>
        ///     The list of channels for which the message shall be transcoded to.
        /// </summary>
#if NET7_0_OR_GREATER
        public required List<ConversationChannel> Channels { get; set; }
#else
        public List<ConversationChannel> Channels { get; set; } = null!;
#endif

        /// <summary>
        ///     Optional.
        /// </summary>
        public string? From { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public string? To { get; set; }
    }
}
