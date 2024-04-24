using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Sinch.Tests.Voice
{
    public class HooksTests
    {
        [Fact]
        public void ValidateRequest()
        {
            // https://developers.sinch.com/docs/voice/api-reference/authentication/callback-signed-request/
            // full path: "https://callbacks.yourdomain.com/sinch/callback/ace"
            var voiceClient = new SinchClient(default, default, default).Voice("669E367E-6BBA-48AB-AF15-266871C28135",
                "BeIukql3pTKJ8RGL5zo0DA==");
            var body =
                "{\"event\":\"ace\",\"callid\":\"822aa4b7-05b4-4d83-87c7-1f835ee0b6f6_257\",\"timestamp\":\"2014-09-24T10:59:41Z\",\"version\":1}";

            voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                new Dictionary<string, StringValues>()
                {
                    { "x-timestamp", new[] { "2014-09-24T10:59:41Z" } },
                    { "content-type", new[] { "application/json" } },
                    {
                        "authorization",
                        new[]
                        {
                            "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4="
                        }
                    }
                }
                , JsonNode.Parse(body)!.AsObject()).Should().BeTrue();
        }
    }
}
