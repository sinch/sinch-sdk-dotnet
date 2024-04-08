using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation
{
    /// <summary>
    ///     Specifies the message source for which the request will be processed.
    ///     Used for operations on messages in Dispatch Mode. For more information,
    ///     see <see href="https://developers.sinch.com/docs/conversation/processing-modes/">Processing Modes</see>.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<MessageSource>))]
    public record MessageSource(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The default messages source. Retrieves messages sent in the default CONVERSATION processing mode,
        ///     which associates the messages with a specific conversation and contact.
        /// </summary>
        public static readonly MessageSource ConversationSource = new("CONVERSATION_SOURCE");

        /// <summary>
        ///     Retrieves messages sent in the DISPATCH processing mode.
        ///     These types of messages are not associated with any conversation or contact.
        /// </summary>
        public static readonly MessageSource DispatchSource = new("DISPATCH_SOURCE");
    }
}


