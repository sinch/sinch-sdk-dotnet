using System.Text;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     The retention policy configured for messages in [Dispatch Mode]
    ///     (https://developers.sinch.com/docs/conversation/processing-modes/).
    ///     Currently only &#x60;MESSAGE_EXPIRE_POLICY&#x60; is available.
    ///     For more information about retention policies,
    ///     see [Retention Policy](https://developers.sinch.com/docs/conversation/keyconcepts/#retention-policy).
    /// </summary>
    public sealed class DispatchRetentionPolicy
    {
        /// <summary>
        /// Gets or Sets RetentionType
        /// </summary>
        public DispatchRetentionPolicyType? RetentionType { get; set; }

        /// <summary>
        ///     Optional. The days before a message is eligible for deletion.
        ///     The valid range is &#x60;[0 - 7]&#x60;. In the case of a &#x60;0&#x60; day TTL,
        ///     messages aren&#39;t stored at all. Note the retention cleanup job runs once every twenty-four hours,
        ///     so messages are not deleted on the minute they become eligible for deletion.
        /// </summary>
        public long TtlDays { get; set; }
        

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class DispatchRetentionPolicy {\n");
            sb.Append("  RetentionType: ").Append(RetentionType).Append("\n");
            sb.Append("  TtlDays: ").Append(TtlDays).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
