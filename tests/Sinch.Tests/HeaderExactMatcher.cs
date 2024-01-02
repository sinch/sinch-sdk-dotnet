using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using RichardSzalay.MockHttp;

namespace Sinch.Tests
{
    /// <summary>
    ///     Matches for a specific header with an exact header value.
    /// </summary>
    public class HeaderExactMatcher : IMockedRequestMatcher
    {
        private readonly string _headerKey;
        private readonly IEnumerable<string> _headerValues;

        public HeaderExactMatcher(string headerKey, IEnumerable<string> headerValues)
        {
            _headerKey = headerKey;
            _headerValues = headerValues;
        }

        public bool Matches(HttpRequestMessage message)
        {
            return message.Headers.GetValues(_headerKey).SequenceEqual(_headerValues);
        }
    }
}
