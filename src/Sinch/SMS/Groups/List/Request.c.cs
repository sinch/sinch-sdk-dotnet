using System.Collections.Generic;
using Sinch.Core;

namespace Sinch.SMS.Groups.List
{
    public sealed class Request
    {
        public int Page { get; set; }

        public int PageSize { get; set; } = 10;

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();
            kvp.Add(new KeyValuePair<string, string>("page", Page.ToString()));
            kvp.Add(new KeyValuePair<string, string>("page_size", PageSize.ToString()));
            return StringUtils.ToQueryString(kvp);
        }
    }
}
