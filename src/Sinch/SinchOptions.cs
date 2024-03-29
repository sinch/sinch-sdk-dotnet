﻿using System.Net.Http;
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
        public SmsHostingRegion SmsHostingRegion { get; set; } = SmsHostingRegion.Us;

        /// <summary>
        ///     Set's the regions for the Conversation api.
        ///     Defaults to "us"
        /// </summary>
        public ConversationRegion ConversationRegion { get; set; } = ConversationRegion.Us;

        /// <inheritdoc cref="ApiUrlOverrides"/>
        public ApiUrlOverrides ApiUrlOverrides { get; set; }
    }

    /// <summary>
    ///     If you want to set your own url for proxy or testing, you can do it here for each API endpoint.
    /// </summary>
    public sealed class ApiUrlOverrides
    {
        /// <summary>
        ///     Overrides SMS api base url
        /// </summary>
        public string SmsUrl { get; init; }

        /// <summary>
        ///     Overrides Conversation api base url
        /// </summary>
        public string ConversationUrl { get; init; }

        /// <summary>
        ///     Overrides Templates api base url.
        ///     Templates is treated as part of conversation api, but it has another base address.
        /// </summary>
        public string TemplatesUrl { get; init; }

        /// <summary>
        ///     Overrides Voice api base url
        /// </summary>
        public string VoiceUrl { get; init; }

        /// <summary>
        ///     Overrides Verification api base url
        /// </summary>
        public string VerificationUrl { get; init; }

        /// <summary>
        ///     Overrides Auth api base url
        /// </summary>
        public string AuthUrl { get; init; }

        /// <summary>
        ///     Overrides Numbers api base url
        /// </summary>
        public string NumbersUrl { get; init; }
    }
}
