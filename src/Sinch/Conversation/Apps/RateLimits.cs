using System.Text;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     RateLimits
    /// </summary>
    public sealed class RateLimits
    {
        /// <summary>
        ///     The number of inbound messages/events we process per second, from underlying channels to the app.
        ///     The default rate limit is 25.
        /// </summary>
        public long? Inbound { get; set; }


        /// <summary>
        ///     The number of messages/events we process per second,
        ///     from the app to the underlying channels.
        ///     Note that underlying channels may have other rate limits.  The default rate limit is 25.
        /// </summary>
        public long? Outbound { get; set; }


        /// <summary>
        ///     The rate limit of callbacks sent to the webhooks registered for the app.
        ///     Note that if you have multiple webhooks with shared triggers,
        ///     multiple callbacks will be sent out for each triggering event. The default rate limit is 25.
        /// </summary>
        public long? Webhooks { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RateLimits {\n");
            sb.Append("  Inbound: ").Append(Inbound).Append("\n");
            sb.Append("  Outbound: ").Append(Outbound).Append("\n");
            sb.Append("  Webhooks: ").Append(Webhooks).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
