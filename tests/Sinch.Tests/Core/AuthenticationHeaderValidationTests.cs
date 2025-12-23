using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using FluentAssertions;
using Sinch.Verification;
using Sinch.Voice;
using Xunit;

namespace Sinch.Tests.Core
{
    /// <summary>
    ///     Tests Voice and Verification clients which are using Application signed auth for hooks validation
    /// </summary>
    public class AuthenticationHeaderValidationTests
    {
        private readonly ISinchVoiceClient _voiceClient = new SinchClient(new SinchClientConfiguration()
        {
            VoiceConfiguration = new SinchVoiceConfiguration()
            {
                AppKey = "669E367E-6BBA-48AB-AF15-266871C28135",
                AppSecret = "BeIukql3pTKJ8RGL5zo0DA=="
            }
        }).Voice;

        private readonly ISinchVerificationClient _verificationClient = new SinchClient(new SinchClientConfiguration()
        {
            VerificationConfiguration = new SinchVerificationConfiguration()
            {
                AppKey = "669E367E-6BBA-48AB-AF15-266871C28135",
                AppSecret = "BeIukql3pTKJ8RGL5zo0DA=="
            }
        }).Verification;

        private string _body =
            "{\"event\":\"ace\",\"callid\":\"822aa4b7-05b4-4d83-87c7-1f835ee0b6f6_257\",\"timestamp\":\"2014-09-24T10:59:41Z\",\"version\":1}";

        private Dictionary<string, IEnumerable<string>> HeadersFromHttpMessage(HttpResponseMessage message)
        {
            return message.Headers.Concat(message.Content.Headers)
                .ToDictionary(x => x.Key, y => y.Value);
        }

        private (HttpResponseMessage, Dictionary<string, IEnumerable<string>>)
            SetupTestHeaders(string timestamp, string auth, string contentType)
        {
            var message = new HttpResponseMessage();
            message.Headers.Add("x-timestamp", timestamp);
            message.Headers.Add("authorization", auth);
            message.Content.Headers.Add("content-type", contentType);
            var headers = HeadersFromHttpMessage(message);
            return (message, headers);
        }


        private void AssertHeaderValidation(Dictionary<string, IEnumerable<string>> headers,
            HttpResponseMessage message,
            string path,
            HttpMethod httpMethod,
            string body,
            bool expected)
        {
            _voiceClient.ValidateAuthenticationHeader(httpMethod, path,
                headers, body).Should().Be(expected);

            _voiceClient.ValidateAuthenticationHeader(httpMethod, path,
                message.Headers, message.Content.Headers, body).Should().Be(expected);

            _verificationClient.ValidateAuthenticationHeader(httpMethod, path,
                headers, body).Should().Be(expected);

            _verificationClient.ValidateAuthenticationHeader(httpMethod, path,
                message.Headers, message.Content.Headers, body).Should().Be(expected);
        }

        [Fact]
        public void ValidateRequest()
        {
            // https://developers.sinch.com/docs/voice/api-reference/authentication/callback-signed-request/
            // full path: "https://callbacks.yourdomain.com/sinch/callback/ace"
            var (message, headers) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, message, "/sinch/callback/ace", HttpMethod.Post, _body,
                expected: true);
        }

        [Fact]
        public void FailIfInvalidAuthHeaderValue()
        {
            var (message, headers) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:bdJO/XUVvIsb5SlZAKmvfw==",
                "application/json");

            AssertHeaderValidation(headers, message, "/sinch/callback/ace", HttpMethod.Post, _body,
                expected: false);
        }


        [Fact]
        public void FailIfAuthHeaderMissing()
        {
            var (message, headers) = SetupTestHeaders("2014-09-24T10:59:41Z",
                null,
                "application/json");

            AssertHeaderValidation(headers, message, "/sinch/callback/ace", HttpMethod.Post, _body,
                expected: false);
        }

        [Fact]
        public void FailIfInvalidPath()
        {
            var (message, headers) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, message, "/not/that/path", HttpMethod.Post, _body,
                expected: false);
        }

        [Fact]
        public void FailNotThatHttpMethod()
        {
            var (message, headers) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, message, "/sinch/callback/ace", HttpMethod.Get, _body,
                expected: false);
        }

        [Fact]
        public void FailNotThatTimestamp()
        {
            var (message, headers) = SetupTestHeaders("2019-11-03T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, message, "/sinch/callback/ace", HttpMethod.Post, _body,
                expected: false);
        }

        [Fact]
        public void FailNotThatContentType()
        {
            var (message, headers) = SetupTestHeaders("2019-11-03T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "text/html");

            AssertHeaderValidation(headers, message, "/sinch/callback/ace", HttpMethod.Post, _body,
                expected: false);
        }

        [Fact]
        public void FailNotThatBody()
        {
            var newBody = JsonNode.Parse("{\"hello\": \"world\"}")!.ToJsonString();
            var (message, headers) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, message, "/sinch/callback/ace", HttpMethod.Post,
                newBody,
                expected: false);
        }

        [Fact]
        public void FailNotApplicationHeader()
        {
            var (message, headers) = SetupTestHeaders("2014-09-24T10:59:41Z",
                "authorization 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=", // diff is here with `authorization`
                "application/json");

            AssertHeaderValidation(headers, message, "/sinch/callback/ace", HttpMethod.Post,
                _body, expected: false);
        }
    }
}
