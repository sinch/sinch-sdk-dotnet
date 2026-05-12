using System;
using System.Net.Http;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;
using Sinch.SMS.Batches;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Groups;
using Sinch.SMS.Inbounds;
using Sinch.SMS.SinchEvents;

namespace Sinch.SMS
{
    /// <summary>
    ///     Send and receive SMS through a single connection for timely and cost-efficient communications using the Sinch SMS
    ///     API.
    /// </summary>
    public interface ISinchSms
    {
        /// <summary>
        ///     Batches are sets of SMS messages. You can send a single message or many.
        ///     Batches are queued and sent at the rate limit in first-in-first-out order.
        /// </summary>
        ISinchSmsBatches Batches { get; }

        /// <summary>
        ///     Inbounds, or Mobile Originated (MO) messages, are incoming messages.
        ///     Inbound messages can be listed and retrieved like batch messages and
        ///     they can also be delivered as Sinch Events like delivery reports.
        /// </summary>
        ISinchSmsInbounds Inbounds { get; }

        /// <summary>
        ///     A group is a set of phone numbers
        ///     (or <see href="https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628">MSISDNs</see>)
        ///     that can be used as a target when sending an SMS.
        ///     An phone number (MSISDN) can only occur once in a group
        ///     and any attempts to add a duplicate are ignored but not rejected.
        /// </summary>
        ISinchSmsGroups Groups { get; }

        /// <summary>
        ///     uses message statuses and error codes in delivery reports,
        ///     which refer to the state of the SMS batch and can be present in either
        ///     <see
        ///         href="https://developers.sinch.com/docs/sms/api-reference/sms/tag/Delivery-reports/#tag/Delivery-reports/operation/GetDeliveryReportByBatchId">
        ///         Retrieve
        ///         a delivery report
        ///     </see>
        ///     or sent to your event destination.
        /// </summary>
        ISinchSmsDeliveryReports DeliveryReports { get; }

        /// <inheritdoc cref="ISmsSinchEvents" />
        ISmsSinchEvents SinchEvents { get; }
    }

    internal sealed class SmsClient : ISinchSms
    {
        private ISinchSmsBatches? _batches;
        private ISinchSmsDeliveryReports? _deliveryReports;
        private ISinchSmsGroups? _groups;
        private ISinchSmsInbounds? _inbounds;
        private readonly SinchSmsConfiguration _config;

        internal SmsClient(
            SinchSmsConfiguration config,
            string projectId,
            string? smsUrlOverride,
            LoggerFactory? loggerFactory,
            IHttp oauthHttp,
            Func<HttpClient> httpClientAccessor)
        {
            SinchEvents = new SmsSinchEvents(oauthHttp.JsonSerializerOptions, loggerFactory?.Create<ISmsSinchEvents>());
            _config = config;

            if (config.ServicePlanIdConfiguration is ServicePlanIdConfiguration svcPlanConfig)
            {
                var baseAddress = !string.IsNullOrEmpty(smsUrlOverride)
                    ? new Uri(smsUrlOverride)
                    : SinchUrlResolvers.ResolveSmsServicePlanIdUrl(svcPlanConfig);
                var bearerHttp = new Http(
                    new Lazy<ISinchAuth>(new BearerAuth(svcPlanConfig.ApiToken)),
                    httpClientAccessor,
                    loggerFactory?.Create<IHttp>(),
                    SnakeCaseNamingPolicy.Instance);
                InitApiClients(svcPlanConfig.ServicePlanId, baseAddress, loggerFactory, bearerHttp);
            }
            else if (config.Region is not null)
            {
                var baseAddress = !string.IsNullOrEmpty(smsUrlOverride)
                    ? new Uri(smsUrlOverride)
                    : SinchUrlResolvers.ResolveSmsUrl(config);
                InitApiClients(projectId, baseAddress, loggerFactory, oauthHttp);
            }
        }

        private void InitApiClients(string id, Uri baseAddress, LoggerFactory? loggerFactory, IHttp http)
        {
            _batches = new Batches.Batches(id, baseAddress, loggerFactory?.Create<ISinchSmsBatches>(), http);
            _inbounds = new Inbounds.Inbounds(id, baseAddress, loggerFactory?.Create<ISinchSmsInbounds>(), http);
            _groups = new Groups.Groups(id, baseAddress, loggerFactory?.Create<ISinchSmsGroups>(), http);
            _deliveryReports = new DeliveryReports.DeliveryReports(id, baseAddress,
                loggerFactory?.Create<ISinchSmsDeliveryReports>(), http);
        }

        public ISinchSmsBatches Batches => GetRequiredApiClient(_batches);

        public ISinchSmsInbounds Inbounds => GetRequiredApiClient(_inbounds);

        public ISinchSmsGroups Groups => GetRequiredApiClient(_groups);

        public ISinchSmsDeliveryReports DeliveryReports => GetRequiredApiClient(_deliveryReports);

        public ISmsSinchEvents SinchEvents { get; }

        private T GetRequiredApiClient<T>(T? client)
            where T : class
        {
            return client ?? throw CreateApiConfigurationException();
        }

        private InvalidOperationException CreateApiConfigurationException()
        {
            var missing = new System.Collections.Generic.List<string>();

            if (_config.ServicePlanIdConfiguration is null && _config.Region is null)
            {
                missing.Add($"{nameof(SinchSmsConfiguration)}.{nameof(SinchSmsConfiguration.Region)}");
                missing.Add($"{nameof(SinchSmsConfiguration)}.{nameof(SinchSmsConfiguration.ServicePlanIdConfiguration)}");
            }

            var detail = missing.Count > 0
                ? $" Missing: {string.Join(" and ", missing)}."
                : string.Empty;

            return new InvalidOperationException(
                $"SMS API operations require a region or service plan configuration.{detail}");
        }
    }
}
