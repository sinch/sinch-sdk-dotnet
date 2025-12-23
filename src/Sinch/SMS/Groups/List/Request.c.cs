using System.Collections.Generic;
using Sinch.Core;

namespace Sinch.SMS.Groups.List
{
    public sealed class ListGroupsRequest
    {
        public int? Page { get; set; }

        public int? PageSize { get; set; }

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();
            if (Page.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("page", Page.Value.ToString()));
            }
            if (PageSize.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("page_size", PageSize.Value.ToString()));
            }

            return StringUtils.ToQueryString(kvp);
        }
    }
}
