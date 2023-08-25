using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation
{
    /// <summary>
    ///     Specifies the message source for which the request will be processed.
    ///     Used for operations on messages in Dispatch Mode. For more information,
    ///     see <see href="https://developers.sinch.com/docs/conversation/processing-modes/">Processing Modes</see>.
    /// </summary>
    [JsonConverter(typeof(SinchEnumConverter<MessageSource>))]
    public enum MessageSource
    {
        /// <summary>
        ///     The default messages source. Retrieves messages sent in the default CONVERSATION processing mode,
        ///     which associates the messages with a specific conversation and contact.
        /// </summary>
        [EnumMember(Value = "CONVERSATION_SOURCE")]
        ConversationSource = 1,

        /// <summary>
        ///     Retrieves messages sent in the DISPATCH processing mode.
        ///     These types of messages are not associated with any conversation or contact.
        /// </summary>
        [EnumMember(Value = "DISPATCH_SOURCE")]
        DispatchSource = 2,
    }
}


