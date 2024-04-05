using System.Collections.Generic;

namespace Sinch.SMS.DeliveryReports.Get
{
    public sealed class GetDeliveryReportResponse
    {
#pragma warning disable CS1570

        /// <summary>
        ///     The type of webhook for the delivery report.
        ///     Returns a either a full or summary delivery report depending on what was set in the batch.
        ///     <see
        ///         href="https://developers.sinch.com/docs/sms/api-reference/sms/tag/Delivery-reports/#tag/Delivery-reports/operation/GetDeliveryReportByBatchId!in=query&path=type&t=request">
        ///         Learn
        ///         the difference between the two.
        ///     </see>
        /// </summary>
#pragma warning restore CA2200
        public DeliveryReportType Type { get; set; }

        /// <summary>
        ///     The ID of the batch this delivery report belongs to.
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        ///     The total number of messages for the batch
        /// </summary>
        public long TotalMessageCount { get; set; }

        /// <summary>
        ///     The client identifier of the batch this delivery report belongs to, if set when submitting batch.
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        ///     Array with status objects. Only status codes with at least one recipient will be listed.
        /// </summary>
        public IEnumerable<DeliveryReportStatusVerbose> Statuses { get; set; }
    }
}
