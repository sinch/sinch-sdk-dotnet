﻿using System.Net.Http;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;

namespace Sinch.Tests
{
    public class JsonMatcher : IMockedRequestMatcher
    {
        private readonly JToken _json;

        public JsonMatcher(string expectedJson)
        {
            _json = JToken.Parse(expectedJson);
        }

        public bool Matches(HttpRequestMessage message)
        {
            if (message.Content == null) return false;
            var content = message.Content.ReadAsStringAsync().Result;
            var actual = JToken.Parse(content);
            actual.Should().BeEquivalentTo(_json);
            return JToken.DeepEquals(_json, actual);
        }
    }
}