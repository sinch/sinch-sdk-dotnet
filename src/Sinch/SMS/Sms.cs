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
        IBatches Batches { get; }

        /// <summary>
        ///     Inbounds, or Mobile Originated (MO) messages, are incoming messages.
        ///     Inbound messages can be listed and retrieved like batch messages and
        ///     they can also be delivered by callback requests like delivery reports.
        /// </summary>
        IInbounds Inbounds { get; }

        /// <summary>
        ///     A group is a set of phone numbers
        ///     (or <see href="https://community.sinch.com/t5/Glossary/MSISDN/ta-p/7628">MSISDNs</see>)
        ///     that can be used as a target when sending an SMS.
        ///     An phone number (MSISDN) can only occur once in a group
        ///     and any attempts to add a duplicate are ignored but not rejected.
        /// </summary>
        IGroups Groups { get; }

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
        IDeliveryReports DeliveryReports { get; }
    }

    internal class Sms : ISinchSms
    {
        internal Sms(string projectId, Uri baseAddress, LoggerFactory loggerFactory, IHttp http)
        {
            Batches = new Batches.Batches(projectId, baseAddress, loggerFactory?.Create<Batches.Batches>(), http);
            Inbounds = new Inbounds.Inbounds(projectId, baseAddress, loggerFactory?.Create<Inbounds.Inbounds>(), http);
            Groups = new Groups.Groups(projectId, baseAddress, loggerFactory?.Create<Groups.Groups>(), http);
            DeliveryReports = new DeliveryReports.DeliveryReports(projectId, baseAddress,
                loggerFactory?.Create<DeliveryReports.DeliveryReports>(), http);
        }

        public IBatches Batches { get; }

        public IInbounds Inbounds { get; }

        public IGroups Groups { get; }

        public IDeliveryReports DeliveryReports { get; }
    }
}
