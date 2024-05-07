using System;
using System.Text;

namespace Sinch.Conversation.Events.Send
{
    public class SendEventResponse
    {
        /// <summary>
        ///     Accepted timestamp.
        /// </summary>
        public DateTime? AcceptedTime { get; set; }


        /// <summary>
        ///     Event id.
        /// </summary>
        public string? EventId { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SendEventResponse {\n");
            sb.Append("  AcceptedTime: ").Append(AcceptedTime).Append("\n");
            sb.Append("  EventId: ").Append(EventId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
