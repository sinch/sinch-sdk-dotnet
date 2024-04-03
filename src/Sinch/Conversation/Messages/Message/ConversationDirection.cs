using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    /// Represents the conversation direction options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<ConversationDirection>))]
    public record ConversationDirection(string Value) : EnumRecord(Value)
    {
        /// <summary>
        /// Undefined direction.
        /// </summary>
        public static readonly ConversationDirection UndefinedDirection = new("UNDEFINED_DIRECTION");

        /// <summary>
        /// To app direction.
        /// </summary>
        public static readonly ConversationDirection ToApp = new("TO_APP");

        /// <summary>
        /// To contact direction.
        /// </summary>
        public static readonly ConversationDirection ToContact = new("TO_CONTACT");
    }
}
