using System.Collections.Generic;

namespace Sinch.SMS.DeliveryReports
{
    public class DeliveryReportStatusVerbose
    {
        /// <summary>
        ///     The detailed status code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     The simplified status as described in <see cref="Sinch.SMS.DeliveryReports.DeliveryReportStatus" />
        /// </summary>
        public DeliveryReportStatus Status { get; set; }

        /// <summary>
        ///     The number of messages that currently has this code. Will always be at least 1
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     Only for full report. A list of the phone number recipients which messages has this status code.
        /// </summary>
        public IEnumerable<string> Recipients { get; set; }
    }
}
