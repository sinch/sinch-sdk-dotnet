using System.Collections.Generic;
using RichardSzalay.MockHttp;

namespace Sinch.Tests
{
    public static class Extensions
    {
        /// <summary>
        ///     Includes requests contain a set of query string values
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="json">The expected json body</param>
        /// <returns>The <see cref="T:MockedRequest"/> instance</returns>
        public static MockedRequest WithJson(this MockedRequest source, string json)
        {
            source.With(new JsonMatcher(json));

            return source;
        }

        public static MockedRequest WithJson(this MockedRequest source, object obj)
        {
            source.With(new JsonMatcher(obj));

            return source;
        }

        public static MockedRequest WithHeaderExact(this MockedRequest source, string headerKey,
            IEnumerable<string> headerValues)
        {
            source.With(new HeaderExactMatcher(headerKey, headerValues));

            return source;
        }
    }
}
