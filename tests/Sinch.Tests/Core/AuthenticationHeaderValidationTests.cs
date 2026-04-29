#nullable enable
using FluentAssertions;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Sinch.Auth;
using Sinch.Verification.SinchEvents;
using Sinch.Voice.SinchEvents;
using Xunit;

namespace Sinch.Tests.Core
{
    /// <summary>
    ///     Tests Voice and Verification clients which are using Application signed auth for hooks validation
    /// </summary>
    public class AuthenticationHeaderValidationTests
    {
        private static readonly ApplicationSignedAuth ApplicationSignedAuth =
            new("669E367E-6BBA-48AB-AF15-266871C28135", "BeIukql3pTKJ8RGL5zo0DA==");
        
        private readonly VerificationSinchEvents _verificationSinchEvents = new(ApplicationSignedAuth);
        
        private readonly VoiceSinchEvents _voiceSinchEvents = new(
            new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            },
            ApplicationSignedAuth);

        private const string Body =
            "{\"event\":\"ace\",\"callid\":\"822aa4b7-05b4-4d83-87c7-1f835ee0b6f6_257\",\"timestamp\":\"2014-09-24T10:59:41Z\",\"version\":1}";

        private Dictionary<string, IEnumerable<string>> SetupTestHeaders(string timestamp, string? auth, string contentType)
        {
            var headers = new Dictionary<string, IEnumerable<string>>
            {
                { "x-timestamp", new[] { timestamp } },
                { "content-type", new[] { contentType } }
            };
            if (auth != null)
            {
                headers.Add("authorization", new[] { auth });
            }
            return headers;
        }

        private void AssertHeaderValidation(Dictionary<string, IEnumerable<string>> headers,
            string path,
            HttpMethod httpMethod,
            string body,
            bool expected)
        {
            _voiceSinchEvents.ValidateAuthenticationHeader(httpMethod, path,
                headers, body).Should().Be(expected);

            _verificationSinchEvents.ValidateAuthenticationHeader(httpMethod, path,
                headers, body).Should().Be(expected);
        }

        [Fact]
        public void ValidateRequest()
        {
            // https://developers.sinch.com/docs/voice/api-reference/authentication/callback-signed-request/
            // full path: "https://callbacks.yourdomain.com/sinch/callback/ace"
            var headers = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, "/sinch/callback/ace", HttpMethod.Post, Body,
                expected: true);
        }

        [Fact]
        public void FailIfInvalidAuthHeaderValue()
        {
            var headers = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:bdJO/XUVvIsb5SlZAKmvfw==",
                "application/json");

            AssertHeaderValidation(headers, "/sinch/callback/ace", HttpMethod.Post, Body,
                expected: false);
        }

        [Fact]
        public void FailIfAuthHeaderMissing()
        {
            var headers = SetupTestHeaders("2014-09-24T10:59:41Z",
                null,
                "application/json");

            AssertHeaderValidation(headers, "/sinch/callback/ace", HttpMethod.Post, Body,
                expected: false);
        }

        [Fact]
        public void FailIfInvalidPath()
        {
            var headers = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, "/not/that/path", HttpMethod.Post, Body,
                expected: false);
        }

        [Fact]
        public void FailNotThatHttpMethod()
        {
            var headers = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, "/sinch/callback/ace", HttpMethod.Get, Body,
                expected: false);
        }

        [Fact]
        public void FailNotThatTimestamp()
        {
            var headers = SetupTestHeaders("2019-11-03T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, "/sinch/callback/ace", HttpMethod.Post, Body,
                expected: false);
        }

        [Fact]
        public void FailNotThatContentType()
        {
            var headers = SetupTestHeaders("2019-11-03T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "text/html");

            AssertHeaderValidation(headers, "/sinch/callback/ace", HttpMethod.Post, Body,
                expected: false);
        }

        [Fact]
        public void FailNotThatBody()
        {
            var newBody = JsonNode.Parse("{\"hello\": \"world\"}")!.ToJsonString();
            var headers = SetupTestHeaders("2014-09-24T10:59:41Z",
                "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, "/sinch/callback/ace", HttpMethod.Post,
                newBody,
                expected: false);
        }

        [Fact]
        public void FailNotApplicationHeader()
        {
            var headers = SetupTestHeaders("2014-09-24T10:59:41Z",
                "authorization 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4=",
                "application/json");

            AssertHeaderValidation(headers, "/sinch/callback/ace", HttpMethod.Post,
                Body, expected: false);
        }
    }
}
