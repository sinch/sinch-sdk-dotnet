using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Messages
{
    /// <summary>
    ///     Represents the message queue priority options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<MessageQueue>))]
    public record MessageQueue(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     Selects normal priority for the message.
        /// </summary>
        public static readonly MessageQueue NormalPriority = new("NORMAL_PRIORITY");

        /// <summary>
        ///     Selects high priority for the message.
        /// </summary>
        public static readonly MessageQueue HighPriority = new("HIGH_PRIORITY");
    }
}
