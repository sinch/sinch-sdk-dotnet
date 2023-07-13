using System.Collections.Generic;

namespace Sinch.SMS.DeliveryReports.List
{
    public sealed class Response
    {
        /// <summary>
        ///     The total number of entries matching the given filters.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     The requested page.
        /// </summary>
        public int Page { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<DeliveryReport> DeliveryReports { get; set; }
    }
}
