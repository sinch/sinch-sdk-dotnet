using System;
using System.Collections.Generic;
using System.Linq;
using Sinch.Core;

namespace Sinch.SMS.Batches.List
{
    public sealed class ListBatchesRequest
    {
        /// <summary>
        ///     The page number starting from 0.
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        ///     Determines the size of a page.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Only list messages sent from this sender number.
        ///     Must be phone numbers or short code.
        /// </summary>
        public IList<string> From { get; set; }

        /// <summary>
        ///     Only list messages received at or after this date/time.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Only list messages received before this date/time.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     Client reference to include
        /// </summary>
        public string ClientReference { get; set; }

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();
            kvp.Add(new KeyValuePair<string, string>("page", Page.ToString()));
            
            if (PageSize.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("page_size", PageSize.ToString()));
            }

            if (From?.Any() is true) kvp.Add(new KeyValuePair<string, string>("from", string.Join(',', From)));

            if (StartDate.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("start_date", StringUtils.ToIso8601(StartDate.Value)));
            }

            if (EndDate.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("end_date", StringUtils.ToIso8601(EndDate.Value)));
            }

            if (ClientReference is not null)
                kvp.Add(new KeyValuePair<string, string>("client_reference", ClientReference));

            return StringUtils.ToQueryString(kvp);
        }
    }
}
