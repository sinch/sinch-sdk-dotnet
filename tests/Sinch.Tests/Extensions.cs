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
    }
}
