using System.Collections.Generic;
using Sinch.Conversation.Messages.Message;

namespace Sinch.Conversation.Transcoding
{
    /// <summary>
    ///     The message to be transcoded, and the app and channels for which the message is to be transcoded.
    /// </summary>
    public class TranscodeRequest
    {

        public required string AppId { get; set; }


        /// <summary>
        ///     Message originating from an app
        /// </summary>

        public required AppMessage AppMessage { get; set; }


        /// <summary>
        ///     The list of channels for which the message shall be transcoded to.
        /// </summary>

        public required List<ConversationChannel> Channels { get; set; }


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
