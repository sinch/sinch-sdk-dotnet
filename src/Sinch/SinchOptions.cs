using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Sinch.Conversation;
using Sinch.Fax;
using Sinch.SMS;

namespace Sinch
{
    public sealed class SinchOptions
    {
        /// <summary>
        ///     A logger factory used to create ILogger inside the SDK to enable logging
        /// </summary>
        public ILoggerFactory? LoggerFactory { get; set; }

        /// <summary>
        ///     A HttpClient to use. If not provided, HttpClient will be created and managed by <see cref="SinchClient"></see>
        ///     itself
        /// </summary>
        public HttpClient? HttpClient { get; set; }

        /// <summary>
        ///     Set's the region for the SMS service.
        ///     <br/><br/>
        ///     The difference between this option and
        ///     <see href="https://developers.sinch.com/docs/sms/api-reference/#base-url">SMS base URL</see>
        ///     is that your account is NOT region locked because SDK utilizes `project_id` API set instead of `service_plan_id`,
        ///     and utilizes region to store the data.
        ///     <br /><br />
        ///     Defaults to "us"
        /// </summary>
        public SmsRegion SmsRegion { get; set; } = SmsRegion.Us;

        /// <summary>
        ///     Set's the regions for the Conversation api.
        ///     Defaults to "us"
        /// </summary>
        public ConversationRegion ConversationRegion { get; set; } = ConversationRegion.Us;

        /// <summary>
        ///     Set's the regions for the Fax api.
        /// </summary>
        public FaxRegion? FaxRegion { get; set; }

        /// <inheritdoc cref="ApiUrlOverrides"/>
        public ApiUrlOverrides? ApiUrlOverrides { get; set; }


        internal ServicePlanIdOptions? ServicePlanIdOptions { get; private set; }

        /// <summary>
        ///     Use SMS API with `service plan id` and compatible region.
        ///     `service_plan_id` will be used in place of `project_id`
        /// </summary>
        /// <param name="servicePlanId">Your service plan id</param>
        /// <param name="apiToken"></param>
        /// <param name="servicePlanIdRegion">Region to use. Defaults to <see cref="SmsServicePlanIdRegion.Us" /></param>
        /// <exception cref="ArgumentNullException">throws if service plan id or region is null or an empty string</exception>
        public void UseServicePlanIdWithSms(string servicePlanId,
            string apiToken, SmsServicePlanIdRegion? servicePlanIdRegion = default)
        {
            servicePlanIdRegion ??= SmsServicePlanIdRegion.Us;

            ServicePlanIdOptions = new ServicePlanIdOptions(servicePlanId, servicePlanIdRegion, apiToken);
        }
    }

    internal sealed class ServicePlanIdOptions
    {
        public ServicePlanIdOptions(string servicePlanId, SmsServicePlanIdRegion region, string apiToken)
        {
            if (string.IsNullOrEmpty(servicePlanId))
            {
                throw new ArgumentNullException(nameof(servicePlanId), "Should have a value");
            }

            if (region is null)
            {
                throw new ArgumentNullException(nameof(region), "Should have a value");
            }

            if (string.IsNullOrEmpty(apiToken))
            {
                throw new ArgumentNullException(nameof(apiToken), "Should have a value");
            }

            ServicePlanId = servicePlanId;
            Region = region;
            ApiToken = apiToken;
        }

        public string ApiToken { get; }

        public SmsServicePlanIdRegion Region { get; }

        public string ServicePlanId { get; }
    }

    /// <summary>
    ///     If you want to set your own url for proxy or testing, you can do it here for each API endpoint.
    /// </summary>
    public sealed class ApiUrlOverrides
    {
        /// <summary>
        ///     Overrides SMS api base url
        /// </summary>
        public string? SmsUrl { get; init; }

        /// <summary>
        ///     Overrides Conversation api base url
        /// </summary>
        public string? ConversationUrl { get; init; }

        /// <summary>
        ///     Overrides Templates api base url.
        ///     Templates are treated as part of conversation api, but it has another base address.
        /// </summary>
        public string? TemplatesUrl { get; init; }

        /// <summary>
        ///     Overrides Voice api base url
        /// </summary>
        public string? VoiceUrl { get; init; }

        /// <summary>
        ///     Overrides Voice api application management base url.
        ///     Voice Application Management is treated as voice api, but it has another base address.
        /// </summary>
        public string? VoiceApplicationManagementUrl { get; init; }

        /// <summary>
        ///     Overrides Verification api base url
        /// </summary>
        public string? VerificationUrl { get; init; }

        /// <summary>
        ///     Overrides Auth api base url
        /// </summary>
        public string? AuthUrl { get; init; }

        /// <summary>
        ///     Overrides Numbers api base url
        /// </summary>
        public string? NumbersUrl { get; init; }

        /// <summary>
        ///     Overrides Fax api base url
        /// </summary>
        public string? FaxUrl { get; init; }
    }
}
