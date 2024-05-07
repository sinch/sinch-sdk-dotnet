using System.Collections.Generic;

namespace Sinch.Numbers
{
    public class NumberPattern
    {
        /// <summary>
        ///     Sequence of digits to search for. If you prefer or need certain digits in sequential order,
        ///     you can enter the sequence of numbers here.
        ///     <example>"2020"</example>
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Pattern { get; set; }
#else
        public string Pattern { get; set; } = null!;
#endif

        /// <summary>
        ///     Search pattern to apply. The options are, START, CONTAIN, and END.
        /// </summary>
        public SearchPattern? SearchPattern { get; set; }

        internal IEnumerable<KeyValuePair<string, string>> GetQueryParamPairs()
        {
            var list = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(Pattern))
            {
                list.Add(new KeyValuePair<string, string>("numberPattern.pattern", Pattern));
            }

            if (SearchPattern is not null)
            {
                list.Add(new KeyValuePair<string, string>("numberPattern.searchPattern",
                    SearchPattern.Value));
            }

            return list;
        }
    }

    /// <summary>
    /// Represents the search pattern options for phone numbers.
    /// </summary>
    public record SearchPattern(string Value)
    {
        /// <summary>
        /// Numbers that begin with the number pattern entered. Often used to search for a specific area code. 
        /// When using START, a plus sign (+) must be included and URL encoded, so %2B. 
        /// For example, to search for area code 206 in the US, you would enter, %2b1206.
        /// </summary>
        public static readonly SearchPattern Start = new("START");

        /// <summary>
        /// The number pattern entered is contained somewhere in the number, the location being undefined.
        /// </summary>
        public static readonly SearchPattern Contain = new("CONTAINS");

        /// <summary>
        /// The number ends with the number pattern entered.
        /// </summary>
        public static readonly SearchPattern End = new("END");
    }
}
