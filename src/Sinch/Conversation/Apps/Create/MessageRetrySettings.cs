using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps.Create
{
    /// <summary>
    ///     This object contains settings related to message retry mechanism.
    /// </summary>
    public sealed class MessageRetrySettings
    {
        /// <summary>
        ///     The maximum duration, in seconds, during which the system will retry sending a message in the event of a temporary processing failure. Time is counted after the first message processing failure. At least one retry is guaranteed. Subsequent retry instances are randomized with exponential backoff. If the next retry timestamp exceeds the configured time, one final retry will be performed on the cut-off time. The valid values for this field are [30 - 3600].
        /// </summary>
        [JsonPropertyName("retry_duration")]
        public int? RetryDuration { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(MessageRetrySettings)} {{\n");
            sb.Append($"  {nameof(RetryDuration)}: ").Append(RetryDuration).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
