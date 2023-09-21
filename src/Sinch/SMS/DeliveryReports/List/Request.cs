using System;
using System.Collections.Generic;
using System.Linq;
using Sinch.Core;

namespace Sinch.SMS.DeliveryReports.List
{
    public sealed class Request
    {
        public int Page { get; set; }

        public int? PageSize { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<DeliveryReportStatus> Status { get; set; }

        public List<string> Code { get; set; }

        public string ClientReference { get; set; }

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();
            kvp.Add(new KeyValuePair<string, string>("page", Page.ToString()));

            if (PageSize.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("page_size", PageSize.Value.ToString()));
            }


            if (StartDate.HasValue)
                kvp.Add(new KeyValuePair<string, string>("start_date", StringUtils.ToIso8601(StartDate.Value)));

            if (EndDate.HasValue)
                kvp.Add(new KeyValuePair<string, string>("end_date", StringUtils.ToIso8601(EndDate.Value)));

            if (Status is not null)
                kvp.Add(new KeyValuePair<string, string>("status", string.Join(",", Status.Select(x => x.Value))));

            if (Code is not null) kvp.Add(new KeyValuePair<string, string>("code", string.Join(",", Code)));

            if (ClientReference is not null)
                kvp.Add(new KeyValuePair<string, string>("client_reference", ClientReference));

            return StringUtils.ToQueryString(kvp);
        }
    }
}
