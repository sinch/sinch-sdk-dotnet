using System;

namespace Sinch.SMS.DeliveryReports
{
    public class DeliveryReport
    {
        /// <summary>
        ///     A timestamp of when the Delivery Report was created in the Sinch service.
        /// </summary>
        public DateTime At { get; set; }

        /// <summary>
        ///     The ID of the batch this delivery report belongs to
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        ///     The detailed status code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     The status field describes which state a particular message is in.
        ///     Note that statuses of type Intermediate will only be reported
        ///     if you request a status per recipient no callback will be made to report them.
        /// </summary>
        public DeliveryReportStatus Status { get; set; }

        /// <summary>
        ///     The default originator used for the recipient this delivery report belongs to,
        ///     if default originator pool configured and no originator set when submitting batch.
        /// </summary>
        public string AppliedOriginator { get; set; }

        /// <summary>
        ///     The client identifier of the batch this delivery report belongs to, if set when submitting batch.
        /// </summary>
        public string ClientReference { get; set; }

        /// <summary>
        ///     The number of parts the message was split into. Present only if max_number_of_message_parts parameter was set.
        /// </summary>
        public int? NumberOfMessageParts { get; set; }

        /// <summary>
        ///     The operator that was used for delivering the message to this recipient, if enabled on the account by Sinch.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        ///     A timestamp extracted from the Delivery Receipt from the originating SMSC.
        /// </summary>
        public DateTime? OperatorStatusAt { get; set; }

        /// <summary>
        ///     The object type. Will always be recipient_delivery_report_sms.
        /// </summary>
        public string Type { get; set; }
    }
}
