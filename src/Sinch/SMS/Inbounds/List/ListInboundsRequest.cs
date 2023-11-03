using System;
using System.Collections.Generic;
using System.Linq;
using Sinch.Core;

namespace Sinch.SMS.Inbounds.List
{
    public sealed class ListInboundsRequest
    {
        /// <summary>
        ///     The page number starting from 0.
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        ///     Determines the size of a page
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Only list messages sent to this destination.
        ///     Multiple phone numbers formatted as either
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see>
        ///     or short codes can be comma separated.
        /// </summary>
        public IList<string> To { get; set; }

        /// <summary>
        ///     Only list messages received at or after this date/time.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Only list messages received before this date/time.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     Using a client reference in inbound messages requires additional setup on your account.
        ///     Contact your <see href="https://dashboard.sinch.com/settings/account-details">account manager</see>
        ///     to enable this feature.<br /><br />
        ///     Only list inbound messages that are in response to messages with a previously provided client reference.
        /// </summary>
        public string ClientReference { get; set; }

        internal string GetQueryString()
        {
            var kvp = new List<KeyValuePair<string, string>>();
            kvp.Add(new KeyValuePair<string, string>("page", Page.ToString()));

            if (PageSize.HasValue)
            {
                kvp.Add(new KeyValuePair<string, string>("page_size", PageSize.Value.ToString()));
            }

            if (To is not null && To.Any()) kvp.Add(new KeyValuePair<string, string>("to", string.Join(',', To)));

            if (StartDate.HasValue)
                kvp.Add(new KeyValuePair<string, string>("start_date", StringUtils.ToIso8601(StartDate.Value)));

            if (EndDate.HasValue)
                kvp.Add(new KeyValuePair<string, string>("end_date", StringUtils.ToIso8601(EndDate.Value)));

            if (ClientReference is not null)
                kvp.Add(new KeyValuePair<string, string>("client_reference", ClientReference));

            return StringUtils.ToQueryString(kvp);
        }
    }
}
