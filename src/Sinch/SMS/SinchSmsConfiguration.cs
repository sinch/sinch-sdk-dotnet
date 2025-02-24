using System;

namespace Sinch.SMS
{
    public sealed class SinchSmsConfiguration
    {
        public string? UrlOverride { get; init; }

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
        public SmsRegion Region { get; init; } = SmsRegion.Us;

        internal ServicePlanIdConfiguration? ServicePlanIdConfiguration { get; set; }

        public static SinchSmsConfiguration WithServicePlanId(string servicePlanId,
            string apiToken, SmsServicePlanIdRegion? servicePlanIdRegion = null,
            string? urlOverride = null)
        {
            return new SinchSmsConfiguration()
            {
                ServicePlanIdConfiguration = new ServicePlanIdConfiguration()
                {
                    ServicePlanIdRegion = servicePlanIdRegion ?? SmsServicePlanIdRegion.Us,
                    ServicePlanId = servicePlanId,
                    UrlOverride = urlOverride,
                    ApiToken = apiToken
                }
            };
        }

        public Uri ResolveUrl()
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!string.IsNullOrEmpty(UrlOverride)) return new Uri(UrlOverride);

            // General SMS rest api uses service_plan_id to performs calls
            // But SDK is based on single-account model which uses project_id
            // Thus, baseAddress for sms api is using a special endpoint where service_plan_id is replaced with projectId
            // for each provided endpoint
            const string smsApiUrlTemplate = "https://zt.{0}.sms.api.sinch.com";
            return new Uri(string.Format(smsApiUrlTemplate, Region.Value));
        }
    }

    internal sealed class ServicePlanIdConfiguration
    {
        internal string? UrlOverride { get; init; }

        internal required string ServicePlanId { get; init; }

        internal required SmsServicePlanIdRegion ServicePlanIdRegion { get; init; }

        internal required string ApiToken { get; init; }

        public Uri ResolveUrl()
        {
            if (!string.IsNullOrEmpty(UrlOverride)) return new Uri(UrlOverride);

            const string smsApiServicePlanIdUrlTemplate = "https://{0}.sms.api.sinch.com";
            return new Uri(string.Format(smsApiServicePlanIdUrlTemplate,
                ServicePlanIdRegion.Value.ToLowerInvariant()));
        }
    }
}
