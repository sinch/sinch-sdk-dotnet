using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace Sinch.Core
{
    internal static class StringUtils
    {
        public static string ToSnakeCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
                .ToLower();
        }

        public static string ToQueryString(IEnumerable<KeyValuePair<string, string>> queryParams, bool encode = true)
        {
            return string.Join("&", queryParams.Select(kvp =>
            {
                var value = encode ? WebUtility.UrlEncode(kvp.Value) : kvp.Value;
                return $"{kvp.Key}={value}";
            }));
        }

        public static string ToIso8601(DateTime date)
        {
            return date.ToString("O", CultureInfo.InvariantCulture);
        }
    }
}
