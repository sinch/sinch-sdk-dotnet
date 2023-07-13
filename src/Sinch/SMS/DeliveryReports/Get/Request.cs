using System.Collections.Generic;
using Sinch.Core;

namespace Sinch.SMS.DeliveryReports.Get
{
    public sealed class Request
    {
        /// <summary>
        ///     The batch ID you received from sending a message.
        ///     <example>01FC66621XXXXX119Z8PMV1QPQ</example>
        /// </summary>
#if NET7_0_OR_GREATER
        public required string BatchId { get; set; }
#else
        public string BatchId { get; set; }
#endif

        public DeliveryReportType DeliveryReportType { get; set; }

        /// <summary>
        ///     A list of <see cref="DeliveryReportStatus" /> to include.
        /// </summary>
        public HashSet<DeliveryReportStatus> Statuses { get; set; }

        /// <summary>
        ///     A list of delivery_receipt_error_codes to include.
        /// </summary>
        public HashSet<string> Code { get; set; }

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();
            kvp.Add(new KeyValuePair<string, string>("type", DeliveryReportType.ToString().ToLowerInvariant()));
            kvp.Add(new KeyValuePair<string, string>("status", string.Join(",", Statuses)));
            kvp.Add(new KeyValuePair<string, string>("code", string.Join(",", Code)));
            return StringUtils.ToQueryString(kvp);
        }
    }
}
