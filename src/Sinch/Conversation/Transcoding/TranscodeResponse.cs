using System.Collections.Generic;

namespace Sinch.Conversation.Transcoding
{
    public class TranscodeResponse
    {
        public Dictionary<ConversationChannel, string> TranscodedMessage { get; set; }
    }
}
