using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     Represents the retention type options for Conversation API.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<RetentionType>))]
    public record RetentionType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The default retention policy where messages older than ttl_days are automatically deleted from Conversation API
        ///     database.
        /// </summary>
        public static readonly RetentionType MessageExpirePolicy = new("MESSAGE_EXPIRE_POLICY");

        /// <summary>
        ///     The conversation expire policy only considers the last message in a conversation.
        ///     If the last message is older than ttl_days, the entire conversation is deleted.
        ///     The difference with MESSAGE_EXPIRE_POLICY is that messages with accept_time older than ttl_days are persisted
        ///     as long as there is a newer message in the same conversation.
        /// </summary>
        public static readonly RetentionType ConversationExpirePolicy = new("CONVERSATION_EXPIRE_POLICY");

        /// <summary>
        ///     Persist policy does not delete old messages or conversations.
        ///     Please note that message storage might be subject to additional charges in the future.
        /// </summary>
        public static readonly RetentionType PersistRetentionPolicy = new("PERSIST_RETENTION_POLICY");
    }
}
