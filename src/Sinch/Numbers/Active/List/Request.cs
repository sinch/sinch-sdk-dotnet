using System.Collections.Generic;
using System.Linq;
using System.Net;
using Sinch.Core;

namespace Sinch.Numbers.Active.List
{
    public sealed class Request
    {
        /// <summary>
        ///     Region code to filter by. ISO 3166-1 alpha-2 country code of the phone number. <br /><br />
        ///     <example>US, GB or SE.</example>
        /// </summary>
#if NET7_0_OR_GREATER
        public required string RegionCode { get; set; }
#else
        public string RegionCode { get; init; } = null!;
#endif

        /// <summary>
        ///     Number type to filter by.
        /// </summary>
#if NET7_0_OR_GREATER
        public required Types Type { get; set; }
#else
        public Types Type { get; set; }
#endif


        /// <summary>
        ///     Search numbers by pattern
        /// </summary>
        public NumberPattern NumberPattern { get; set; }

        /// <summary>
        ///     Number capabilities to filter by SMS and/or VOICE.
        /// </summary>
        public IList<Product> Capability { get; set; }

        /// <summary>
        ///     The maximum number of items to return.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     The next page token value returned from a previous List request, if any.
        /// </summary>
        public string PageToken { get; set; }

        /// <summary>
        ///     Supported fields for ordering by phoneNumber or displayName.
        /// </summary>
        public OrderBy OrderBy { get; set; }

        internal string GetQueryString()
        {
            var dict = new List<KeyValuePair<string, string>>
            {
                new("regionCode", RegionCode),
                new("type", Type.Value)
            };

            if (NumberPattern != null)
            {
                dict.Add(new KeyValuePair<string, string>("numberPattern.pattern", NumberPattern.Pattern));
                if (NumberPattern.SearchPattern != null)
                    dict.Add(new KeyValuePair<string, string>("numberPattern.searchPattern",
                        NumberPattern.SearchPattern.Value.ToUpperInvariant()));
            }

            if (Capability is not null)
            {
                dict.AddRange(Capability.Select(i =>
                    new KeyValuePair<string, string>("capability", i.Value.ToUpperInvariant())));
            }

            if (PageSize is not null) dict.Add(new KeyValuePair<string, string>("pageSize", PageSize.Value.ToString()));

            if (PageToken != null) dict.Add(new KeyValuePair<string, string>("pageToken", PageToken));

            if (OrderBy is not null)
                dict.Add(new KeyValuePair<string, string>("orderBy", OrderBy.Value));

            return string.Join("&", dict.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));
        }
    }
}
