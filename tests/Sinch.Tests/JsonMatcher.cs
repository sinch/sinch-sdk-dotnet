using System.Net.Http;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;

namespace Sinch.Tests
{
    public class JsonMatcher : IMockedRequestMatcher
    {
        private readonly JToken _expectedJson;

        /// <summary>
        ///     Initialized a JsonMatcher from anonymous object
        /// </summary>
        /// <param name="obj"></param>
        public JsonMatcher(object obj)
        {
            _expectedJson = JToken.FromObject(obj);
        }

        public JsonMatcher(string expectedJson)
        {
            _expectedJson = JToken.Parse(expectedJson);
        }

        public bool Matches(HttpRequestMessage message)
        {
            if (message.Content == null) return false;
            var content = message.Content.ReadAsStringAsync().Result;
            var actual = JToken.Parse(content);
            actual.Should().BeEquivalentTo(_expectedJson);
            return JToken.DeepEquals(_expectedJson, actual);
        }
    }
}
