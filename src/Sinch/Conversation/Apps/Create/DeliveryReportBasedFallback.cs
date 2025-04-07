using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps.Create
{
    /// <summary>
    ///     This object contains additional settings related to [delivery report based fallback](https://developers.sinch.com/docs/conversation/keyconcepts/#delivery-report-base-message-fallback). Note that this **paid** functionality is available for open beta testing.
    /// </summary>
    public sealed class DeliveryReportBasedFallback
    {
        /// <summary>
        ///     Optional. A flag specifying whether this app has enabled fallback message delivery upon no positive delivery report. This feature is applicable only to messages which are sent to a recipient with more than one channel identity. Identities must be defined on channels which support at least the &#39;DELIVERED&#39; message state. **Please note that this functionality requires payment.**
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }


        /// <summary>
        ///     Optional. The time, in seconds, after which a message without a positive delivery report will fallback to the next channel. The valid values for this field are [60 - 259200].
        /// </summary>
        [JsonPropertyName("delivery_report_waiting_time")]
        public int? DeliveryReportWaitingTime { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(DeliveryReportBasedFallback)} {{\n");
            sb.Append($"  {nameof(Enabled)}: ").Append(Enabled).Append('\n');
            sb.Append($"  {nameof(DeliveryReportWaitingTime)}: ").Append(DeliveryReportWaitingTime).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
