using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Apps
{
    [JsonConverter(typeof(SinchEnumConverter<RetentionType>))]
    public enum RetentionType
    {
        /// <summary>
        ///     The default retention policy where messages older
        ///     than ttl_days are automatically deleted from Conversation API database.
        /// </summary>
        [EnumMember(Value = "MESSAGE_EXPIRE_POLICY")]
        MessageExpirePolicy,

        /// <summary>
        ///     The conversation expire policy only considers the last message in a conversation.
        ///     If the last message is older that ttl_days the entire conversation is deleted.
        ///     The difference with MESSAGE_EXPIRE_POLICY is that messages with accept_time older than
        ///     ttl_days are persisted as long as there is a newer message in the same conversation.
        /// </summary>
        [EnumMember(Value = "CONVERSATION_EXPIRE_POLICY")]
        ConversationExpirePolicy,

        /// <summary>
        ///     Persist policy does not delete old messages or conversations.
        ///     Please note that message storage might be subject to additional charges in the future.
        /// </summary>
        [EnumMember(Value = "PERSIST_RETENTION_POLICY")]
        PersistRetentionPolicy,
    }
}
