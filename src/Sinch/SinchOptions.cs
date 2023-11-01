using System.Net.Http;
using Microsoft.Extensions.Logging;
using Sinch.Conversation;
using Sinch.SMS;

namespace Sinch
{
    public sealed class SinchOptions
    {
        /// <summary>
        ///     A logger factory used to create ILogger inside the SDK to enable logging
        /// </summary>
        public ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        ///     A HttpClient to use. If not provided, HttpClient will be created and managed by <see cref="SinchClient"></see>
        ///     itself
        /// </summary>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        ///     Set's the hosting region for the SMS service.
        ///     <br/><br/>
        ///     The difference between this option and
        ///     <see href="https://developers.sinch.com/docs/sms/api-reference/#base-url">SMS base URL</see>
        ///     is that your account is NOT region locked because SDK utilizes `project_id` API set instead of `service_plan_id`,
        ///     and utilizes region to store the data.
        ///     <br /><br />
        ///     Defaults to "us"
        /// </summary>
        public SmsRegion SmsHostingRegion { get; set; } = SmsRegion.Us;

        /// <summary>
        ///     Set's the regions for the Conversation api.
        ///     Defaults to "us"
        /// </summary>
        public ConversationRegion ConversationRegion { get; set; } = ConversationRegion.Us;
    }
}
