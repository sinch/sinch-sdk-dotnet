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

         private static string RegionRequiredMessage =>
            $"{nameof(SinchSmsConfiguration)}.{nameof(Region)} is required. " +
            $"Set it to one of the values in {nameof(SmsRegion)}, e.g. {nameof(SmsRegion)}.{nameof(SmsRegion.Us)}.";

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
            if (UrlOverride is not null)
                return new Uri(UrlOverride);

            if (Region is null)
                throw new InvalidOperationException(RegionRequiredMessage);

            return new Uri($"https://zt.{Region.Value}.sms.api.sinch.com");
        }

        internal void Validate()
        {
            if (Region == null)
                throw new InvalidOperationException(RegionRequiredMessage);
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
