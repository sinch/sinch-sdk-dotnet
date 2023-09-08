using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Apps
{
    [JsonConverter(typeof(SinchEnumConverter<DispatchRetentionPolicyType>))]
    public enum DispatchRetentionPolicyType
    {
        /// <summary>
        ///     The default retention policy where messages older
        ///     than ttl_days are automatically deleted from Conversation API database.
        /// </summary>
        [EnumMember(Value = "MESSAGE_EXPIRE_POLICY")]
        MessageExpirePolicy,
    }
}
