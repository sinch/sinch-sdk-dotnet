using System.Text;

namespace Sinch.Conversation.Apps
{
    public class QueueStats
    {
        /// <summary>
        ///     The current size of the App&#39;s MT queue.
        /// </summary>
        public long OutboundSize { get; set; }


        /// <summary>
        ///     The limit of the App&#39;s MT queue. The default limit is 500000 messages.
        /// </summary>
        public long OutboundLimit { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class QueueStats {\n");
            sb.Append("  OutboundSize: ").Append(OutboundSize).Append("\n");
            sb.Append("  OutboundLimit: ").Append(OutboundLimit).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
