using System;
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

        internal bool IsUsingServicePlanId { get; }
    }

    internal record ServicePlanId(string Value);

    internal record ProjectId(string Value);

    internal sealed class SmsClient : ISinchSms
    {
        private readonly ISinchSmsBatches? _batches;
        private readonly ISinchSmsDeliveryReports? _deliveryReports;
        private readonly ISinchSmsGroups? _groups;
        private readonly ISinchSmsInbounds? _inbounds;
        private readonly bool _supportsApiOperations;

        /// <summary>
        ///     Creates an SMS client that supports Sinch Events parsing and signature validation only.
        ///     SMS API operations remain unavailable until SMS configuration is provided.
        /// </summary>
        /// <param name="loggerFactory">Logger factory used to create SMS-related loggers.</param>
        /// <param name="http">HTTP abstraction that provides serializer options used by SMS Sinch Events.</param>
        internal SmsClient(LoggerFactory? loggerFactory, IHttp http)
        {
            SinchEvents = new SmsSinchEvents(
                http.JsonSerializerOptions,
                loggerFactory?.Create<ISmsSinchEvents>());
        }

        /// <summary>
        ///     Creates an instance of Sms service with project id
        /// </summary>
        /// <param name="projectId">Project identifier used in SMS API paths.</param>
        /// <param name="baseAddress">Base address for SMS API requests.</param>
        /// <param name="loggerFactory">Logger factory used to create SMS-related loggers.</param>
        /// <param name="http">HTTP abstraction used for SMS API requests.</param>
        internal SmsClient(ProjectId projectId, Uri baseAddress, LoggerFactory? loggerFactory, IHttp http) : this(
            projectId.Value, baseAddress, loggerFactory, http)
        {
        }

        /// <summary>
        ///     Creates an instance of Sms service with service plan id
        /// </summary>
        /// <param name="servicePlanId">Service plan identifier used in SMS API paths.</param>
        /// <param name="baseAddress">Base address for SMS API requests.</param>
        /// <param name="loggerFactory">Logger factory used to create SMS-related loggers.</param>
        /// <param name="http">HTTP abstraction used for SMS API requests.</param>
        internal SmsClient(ServicePlanId servicePlanId, Uri baseAddress, LoggerFactory? loggerFactory,
            IHttp http) : this(
            servicePlanId.Value, baseAddress, loggerFactory, http)
        {
            IsUsingServicePlanId = true;
        }

        /// <summary>
        ///     Creates an instance of Sms service. Be aware that first parameter is either projectId or servicePlanId.
        ///     They are not distinguished more cause only service_plan_id and project_id is in the same place in url path
        ///     parameters, but base address is different.
        /// </summary>
        /// <param name="projectIdOrServicePlanId">Project ID or service plan ID used in SMS API paths.</param>
        /// <param name="baseAddress">Base address for SMS API requests.</param>
        /// <param name="loggerFactory">Logger factory used to create SMS-related loggers.</param>
        /// <param name="http">HTTP abstraction used for SMS API requests.</param>
        private SmsClient(string projectIdOrServicePlanId, Uri baseAddress, LoggerFactory? loggerFactory, IHttp http)
            : this(loggerFactory, http)
        {
            _supportsApiOperations = true;
            _batches = new Batches.Batches(projectIdOrServicePlanId, baseAddress,
                loggerFactory?.Create<ISinchSmsBatches>(), http);
            _inbounds = new Inbounds.Inbounds(projectIdOrServicePlanId, baseAddress,
                loggerFactory?.Create<ISinchSmsInbounds>(), http);
            _groups = new Groups.Groups(projectIdOrServicePlanId, baseAddress, loggerFactory?.Create<ISinchSmsGroups>(),
                http);
            _deliveryReports = new DeliveryReports.DeliveryReports(projectIdOrServicePlanId, baseAddress,
                loggerFactory?.Create<ISinchSmsDeliveryReports>(), http);
        }

        public ISinchSmsBatches Batches => _supportsApiOperations
            ? _batches!
            : throw CreateApiConfigurationException();

        public ISinchSmsInbounds Inbounds => _supportsApiOperations
            ? _inbounds!
            : throw CreateApiConfigurationException();

        public ISinchSmsGroups Groups => _supportsApiOperations
            ? _groups!
            : throw CreateApiConfigurationException();

        public ISinchSmsDeliveryReports DeliveryReports => _supportsApiOperations
            ? _deliveryReports!
            : throw CreateApiConfigurationException();

        public ISmsSinchEvents SinchEvents { get; }

        public bool IsUsingServicePlanId { get; }

        private static InvalidOperationException CreateApiConfigurationException()
        {
            return new InvalidOperationException(
                "SMS API operations are unavailable when the SMS client is initialized for Sinch Events only. " +
                $"Configure either {nameof(SinchSmsConfiguration)}.{nameof(SinchSmsConfiguration.Region)} or " +
                $"{nameof(SinchSmsConfiguration)}.{nameof(SinchSmsConfiguration.ServicePlanIdConfiguration)} to use SMS API operations.");
        }
    }
}
