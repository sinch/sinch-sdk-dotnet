using System.Collections.Generic;

namespace Sinch.SMS.Batches.List
{
    public class Response
    {
        /// <summary>
        ///     The requested page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        ///     The total number of batches matching the given filters.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     The number of batches returned in this request
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     The page of batches matching the given filters
        /// </summary>
        public List<Batch> Batches { get; set; }
    }
}
