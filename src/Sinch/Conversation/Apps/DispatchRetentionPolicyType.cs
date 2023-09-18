using System.Text.Json.Serialization;
using Sinch.Core;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     Represents the dispatch retention policy type options.
    /// </summary>
    [JsonConverter(typeof(EnumRecordJsonConverter<DispatchRetentionPolicyType>))]
    public record DispatchRetentionPolicyType(string Value) : EnumRecord(Value)
    {
        /// <summary>
        ///     The default retention policy where messages older
        ///     than ttl_days are automatically deleted from the Conversation API database.
        /// </summary>
        public static readonly DispatchRetentionPolicyType MessageExpirePolicy = new("MESSAGE_EXPIRE_POLICY");
    }
}
