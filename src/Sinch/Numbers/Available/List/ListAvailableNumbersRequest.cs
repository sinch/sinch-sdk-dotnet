using System.Collections.Generic;
using System.Linq;
using Sinch.Core;

namespace Sinch.Numbers.Available.List
{
    public sealed class ListAvailableNumbersRequest
    {
        /// <summary>
        ///     Region code to filter by. ISO 3166-1 alpha-2 country code of the phone number.
        ///     <example>US, GB or SE.</example>
        /// </summary>
#if NET7_0_OR_GREATER
        public required string RegionCode { get; set; }
#else
        public string RegionCode { get; set; }
#endif


        /// <summary>
        ///     Number type to filter by. Options include, MOBILE, LOCAL or TOLL_FREE.
        /// </summary>
#if NET7_0_OR_GREATER
        public required Types Type { get; set; }
#else
        public Types Type { get; set; }
#endif


        /// <summary>
        ///     <see cref="NumberPattern" />
        /// </summary>
        public NumberPattern NumberPattern { get; set; }

        /// <summary>
        ///     Number capabilities to filter by SMS and/or VOICE.
        /// </summary>
        public IList<Product> Capabilities { get; set; }

        /// <summary>
        ///     Optional. The maximum number of items to return.
        /// </summary>
        public int? Size { get; set; }

        internal string GetQueryString()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new("regionCode", RegionCode),
                new("type", Type.Value)
            };

            if (NumberPattern != null) list.AddRange(NumberPattern.GetQueryParamPairs());

            if (Capabilities is not null)
            {
                list.AddRange(Capabilities.Select(i =>
                    new KeyValuePair<string, string>("capabilities", i.Value.ToUpperInvariant())));
            }

            if (Size.HasValue) list.Add(new KeyValuePair<string, string>("size", Size.Value.ToString()));

            return StringUtils.ToQueryString(list);
        }
    }
}
