using System;
using Sinch.Core;
using Sinch.Logger;
using Sinch.SMS.Batches;
using Sinch.SMS.DeliveryReports;
using Sinch.SMS.Groups;
using Sinch.SMS.Inbounds;

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
        ///     they can also be delivered by callback requests like delivery reports.
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
        ///     or sent to a callback.
        /// </summary>
        ISinchSmsDeliveryReports DeliveryReports { get; }

        internal bool IsUsingServicePlanId { get; }
    }

    internal record ServicePlanId(string Value);

    internal record ProjectId(string Value);

    internal class SmsClient : ISinchSms
    {
        /// <summary>
        ///     Creates an instance of Sms service with project id
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="baseAddress"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="http"></param>
        internal SmsClient(ProjectId projectId, Uri baseAddress, LoggerFactory loggerFactory, IHttp http) : this(
            projectId.Value, baseAddress, loggerFactory, http)
        {
        }
        
        /// <summary>
        ///     Creates an instance of Sms service with service plan id
        /// </summary>
        /// <param name="servicePlanId"></param>
        /// <param name="baseAddress"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="http"></param>
        internal SmsClient(ServicePlanId servicePlanId, Uri baseAddress, LoggerFactory loggerFactory, IHttp http) : this(
            servicePlanId.Value, baseAddress, loggerFactory, http)
        {
            IsUsingServicePlanId = true;
        }

        /// <summary>
        ///     Creates an instance of Sms service. Be aware that first parameter is either projectId or servicePlanId.
        ///     They are not distinguished more cause only service_plan_id and project_id is in the same place in url path
        ///     parameters, but base address is different.
        /// </summary>
        /// <param name="projectIdOrServicePlanId"></param>
        /// <param name="baseAddress"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="http"></param>
        private SmsClient(string projectIdOrServicePlanId, Uri baseAddress, LoggerFactory loggerFactory, IHttp http)
        {
            Batches = new Batches.Batches(projectIdOrServicePlanId, baseAddress,
                loggerFactory?.Create<ISinchSmsBatches>(), http);
            Inbounds = new Inbounds.Inbounds(projectIdOrServicePlanId, baseAddress,
                loggerFactory?.Create<ISinchSmsInbounds>(), http);
            Groups = new Groups.Groups(projectIdOrServicePlanId, baseAddress, loggerFactory?.Create<ISinchSmsGroups>(),
                http);
            DeliveryReports = new DeliveryReports.DeliveryReports(projectIdOrServicePlanId, baseAddress,
                loggerFactory?.Create<ISinchSmsDeliveryReports>(), http);
        }

        public ISinchSmsBatches Batches { get; }

        public ISinchSmsInbounds Inbounds { get; }

        public ISinchSmsGroups Groups { get; }

        public ISinchSmsDeliveryReports DeliveryReports { get; }

        public bool IsUsingServicePlanId { get; } = false;
    }
}
