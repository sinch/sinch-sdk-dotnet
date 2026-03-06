using System;

namespace Sinch.SMS
{
    public sealed class SinchSmsConfiguration
    {
        public string? UrlOverride { get; init; }

        /// <summary>
        ///     Set the region for the SMS service.
        ///     The difference between this option and
        ///     <see href="https://developers.sinch.com/docs/sms/api-reference/#base-url">SMS base URL</see>
        ///     is that your account is NOT region locked because SDK utilizes `project_id` API set instead of `service_plan_id`,
        ///     and uses a region to store the data.
        ///     Required. See <see cref="SmsRegion" /> for available values.
        /// </summary>
        public SmsRegion? Region { get; init; }

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

        internal Uri ResolveUrl()
        {
            const string smsApiUrlTemplate = "https://zt.{0}.sms.api.sinch.com";
            return new Uri(UrlOverride ?? string.Format(smsApiUrlTemplate, Region!.Value));
        }

        internal void Validate()
        {
            if (Region == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(SinchSmsConfiguration)}.{nameof(Region)} is required. " +
                    $"Set it to one of the values in {nameof(SmsRegion)}, e.g. {nameof(SmsRegion)}.{nameof(SmsRegion.Us)}.");
            }
        }
    }

    internal sealed class ServicePlanIdConfiguration
    {
        internal string? UrlOverride { get; init; }

        internal required string ServicePlanId { get; init; }

        internal required SmsServicePlanIdRegion ServicePlanIdRegion { get; init; }

        internal required string ApiToken { get; init; }

        internal Uri ResolveUrl()
        {
            const string smsApiServicePlanIdUrlTemplate = "https://{0}.sms.api.sinch.com";
            return new Uri(UrlOverride ?? string.Format(smsApiServicePlanIdUrlTemplate,
                ServicePlanIdRegion.Value.ToLowerInvariant()));
        }
    }
}
