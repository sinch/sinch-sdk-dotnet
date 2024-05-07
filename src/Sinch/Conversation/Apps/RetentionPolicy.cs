using System.Text;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     The retention policy configured for the app. For more information about retention policies,
    ///     see [Retention Policy](https://developers.sinch.com/docs/conversation/keyconcepts/#retention-policy).
    /// </summary>
    public sealed class RetentionPolicy
    {
        /// <summary>
        ///     Gets or Sets RetentionType
        /// </summary>
        public RetentionType? RetentionType { get; set; }


        /// <summary>
        ///     Optional. The days before a message or conversation is eligible for deletion.
        ///     Default value is 180. The ttl_days value has no effect when retention_type
        ///     is &#x60;PERSIST_RETENTION_POLICY&#x60;. The valid values for this field are [1 - 3650].
        ///     Note that retention cleanup job runs once every twenty-four hours which can lead to delay i.e.,
        ///     messages and conversations are not deleted on the minute they become eligible for deletion.
        /// </summary>
        public long? TtlDays { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RetentionPolicy {\n");
            sb.Append("  RetentionType: ").Append(RetentionType).Append("\n");
            sb.Append("  TtlDays: ").Append(TtlDays).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
