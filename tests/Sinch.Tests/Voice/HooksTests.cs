using System.Collections.Generic;
using System.Linq;
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
        private readonly ISinchVoiceClient _voiceClient = new SinchClient(default, default, default).Voice(
            "669E367E-6BBA-48AB-AF15-266871C28135",
            "BeIukql3pTKJ8RGL5zo0DA==");

        private string _body =
            "{\"event\":\"ace\",\"callid\":\"822aa4b7-05b4-4d83-87c7-1f835ee0b6f6_257\",\"timestamp\":\"2014-09-24T10:59:41Z\",\"version\":1}";

        private Dictionary<string, IEnumerable<string>> HeadersFromHttpMessage(HttpResponseMessage message)
        {
            return message.Headers.Concat(message.Content.Headers)
                .ToDictionary(x => x.Key, y => y.Value);
        }

        private Dictionary<string, StringValues> StringValuesFromEnumerable(
            Dictionary<string, IEnumerable<string>> headers)
        {
            return headers.ToDictionary(x => x.Key, y => new StringValues(y.Value.ToArray()));
        }

        private (HttpResponseMessage, Dictionary<string, IEnumerable<string>>, Dictionary<string, StringValues>)
            SetupTestHeaders(string timestamp, string auth, string contentType)
        {
            var message = new HttpResponseMessage();
            message.Headers.Add("x-timestamp", timestamp);
            message.Headers.Add("authorization",
                auth);
            message.Content.Headers.Add("content-type", contentType);
            var headers = HeadersFromHttpMessage(message);
            var headersAsStringValues = StringValuesFromEnumerable(headers);
            return (message, headers, headersAsStringValues);
        }

        [Fact]
        public void ValidateRequest()
        {
            // https://developers.sinch.com/docs/voice/api-reference/authentication/callback-signed-request/
            // full path: "https://callbacks.yourdomain.com/sinch/callback/ace"
            var (message, headers, headersStringValues) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            // TODO: remove in 2.0
            _voiceClient
                .ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace", headersStringValues
                    ,
                    JsonNode.Parse(_body)!.AsObject()).Should().BeTrue();
            // test overrides of a method too
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                    headers, _body)
                .Should().BeTrue();

            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace", message.Headers,
                message.Content.Headers, _body).Should().BeTrue();
        }

        [Fact]
        public void FailIfInvalidAuthHeaderValue()
        {
            var (message, headers, headersStringValues) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:bdJO/XUVvIsb5SlZAKmvfw==",
                "application/json");


            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                headers, _body).Should().BeFalse();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                headersStringValues
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                message.Headers, message.Content.Headers, _body).Should().BeFalse();
        }

        [Fact]
        public void FailIfAuthHeaderMissing()
        {
            var (message, headers, headersStringValues) = SetupTestHeaders("2014-09-24T10:59:41Z",
                null,
                "application/json");

            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                headersStringValues
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
            
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                headers, _body).Should().BeFalse();
            
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace",
                message.Headers, message.Content.Headers, _body).Should().BeFalse();
        }

        [Fact]
        public void FailIfInvalidPath()
        {
            var (message, headers, headersStringValues) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            var path = "/not/that/path";
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, path,
                headersStringValues
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
            
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, path,
                headers, _body).Should().BeFalse();
            
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, path,
                message.Headers, message.Content.Headers, _body).Should().BeFalse();
        }

        [Fact]
        public void FailNotThatHttpMethod()
        {
            var (message, headers, headersStringValues) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");
            
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Get, "/sinch/callback/ace",
                headersStringValues
                , JsonNode.Parse(_body)!.AsObject()).Should().BeFalse();
            
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Get, "/sinch/callback/ace",
                headers, _body).Should().BeFalse();
            
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Get, "/sinch/callback/ace",
                message.Headers, message.Content.Headers, _body).Should().BeFalse();
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
