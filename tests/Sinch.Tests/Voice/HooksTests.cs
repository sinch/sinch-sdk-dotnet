using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Sinch.Voice;
using Xunit;

namespace Sinch.Tests.Voice
{
    public class HooksTests
    {
        private readonly ISinchVoiceClient _voiceClient = new SinchClient(new SinchClientConfiguration()
        {
            SinchCommonCredentials = new SinchCommonCredentials()
            {
                ProjectId = "PROJECT_ID",
                KeyId = "KEY_ID",
                KeySecret = "KEY_SECRET",
            },
            VoiceConfiguration = new SinchVoiceConfiguration()
            {
                AppKey = "669E367E-6BBA-48AB-AF15-266871C28135",
                AppSecret = "BeIukql3pTKJ8RGL5zo0DA=="
            }
        }).Voice;

        private string _body =
            "{\"event\":\"ace\",\"callid\":\"822aa4b7-05b4-4d83-87c7-1f835ee0b6f6_257\",\"timestamp\":\"2014-09-24T10:59:41Z\",\"version\":1}";

        [Fact]
        public void ValidateRequest()
        {
            // https://developers.sinch.com/docs/voice/api-reference/authentication/callback-signed-request/
            // full path: "https://callbacks.yourdomain.com/sinch/callback/ace"

            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
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
                , JsonNode.Parse(_body)!.AsObject()).Should().BeTrue();
        }

        [Fact]
        public void FailIfInvalidAuthHeaderValue()
        {
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                new Dictionary<string, StringValues>()
                {
                    { "x-timestamp", new[] { "2014-09-24T10:59:41Z" } },
                    { "content-type", new[] { "application/json" } },
                    {
                        "authorization",
                        new[]
                        {
                            "application 669E367E-6BBA-48AB-AF15-266871C28135:bdJO/XUVvIsb5SlZAKmvfw=="
                        }
                    }
                }
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
        }

        [Fact]
        public void FailIfAuthHeaderMissing()
        {
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                new Dictionary<string, StringValues>()
                {
                    { "x-timestamp", new[] { "2014-09-24T10:59:41Z" } },
                    { "content-type", new[] { "application/json" } },
                }
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
        }

        [Fact]
        public void FailIfInvalidPath()
        {
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/not/that/path",
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
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
        }

        [Fact]
        public void FailNotThatHttpMethod()
        {
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Get, "/sinch/callback/ace",
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
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
        }

        [Fact]
        public void FailNotThatTimestamp()
        {
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                new Dictionary<string, StringValues>()
                {
                    { "x-timestamp", new[] { "2019-11-03T10:59:41Z" } },
                    { "content-type", new[] { "application/json" } },
                    {
                        "authorization",
                        new[]
                        {
                            "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4="
                        }
                    }
                }
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
        }

        [Fact]
        public void FailNotThatContentType()
        {
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                new Dictionary<string, StringValues>()
                {
                    { "x-timestamp", new[] { "2014-09-24T10:59:41Z" } },
                    { "content-type", new[] { "text" } },
                    {
                        "authorization",
                        new[]
                        {
                            "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4="
                        }
                    }
                }
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
        }

        [Fact]
        public void FailNotThatBody()
        {
            _body = JsonNode.Parse("{\"hello\": \"world\"}")!.ToJsonString();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
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
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
        }

        [Fact]
        public void FailNotApplicationHeader()
        {
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                new Dictionary<string, StringValues>()
                {
                    { "x-timestamp", new[] { "2014-09-24T10:59:41Z" } },
                    { "content-type", new[] { "application/json" } },
                    {
                        "authorization",
                        new[]
                        {
                            "authorization 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4="
                        }
                    }
                }
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
        }
    }
}
