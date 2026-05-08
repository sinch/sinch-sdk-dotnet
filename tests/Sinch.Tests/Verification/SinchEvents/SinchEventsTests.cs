using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using FluentAssertions;
using Sinch.Verification;
using Sinch.Verification.SinchEvents;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class SinchEventsTests
    {
        [Fact]
        public void ParseEvent_ReturnsVerificationRequestEvent()
        {
            const string json = """
                {
                  "id": "1234567890",
                  "event": "VerificationRequestEvent",
                  "method": "sms",
                  "identity": {
                    "type": "number",
                    "endpoint": "+11235551234"
                  }
                }
                """;

            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var parsed = sinchEvents.ParseEvent(json);

            parsed.Should().BeOfType<VerificationRequestEvent>()
                .Which.Id.Should().Be("1234567890");
        }

        [Fact]
        public void ParseEvent_ReturnsVerificationResultEvent()
        {
            const string json = """
                {
                  "id": "1234567890",
                  "event": "VerificationResultEvent",
                  "method": "sms",
                  "identity": {
                    "type": "number",
                    "endpoint": "+11235551234"
                  },
                  "status": "SUCCESSFUL"
                }
                """;

            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var parsed = sinchEvents.ParseEvent(json);

            parsed.Should().BeOfType<VerificationResultEvent>()
                .Which.Status.Should().Be(Common.VerificationStatus.Successful);
        }

        [Fact]
        public void ParseEvent_Throws_WhenEventTypeUnknown()
        {
            const string json = """
                {
                  "event": "UnknownVerificationEvent"
                }
                """;

            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            Action act = () => sinchEvents.ParseEvent(json);

            act.Should().Throw<JsonException>()
                .WithMessage("*Unknown Verification Sinch Event type*");
        }

        [Fact]
        public void ValidateAuthenticationHeader_UsesApplicationSignedAuth()
        {
            var sinchEvents = new SinchClient(new SinchClientConfiguration
            {
                VerificationConfiguration = new SinchVerificationConfiguration
                {
                    AppKey = "669E367E-6BBA-48AB-AF15-266871C28135",
                    AppSecret = "BeIukql3pTKJ8RGL5zo0DA=="
                }
            }).Verification.SinchEvents;

            var headers = new Dictionary<string, IEnumerable<string>>
            {
                {
                    "x-timestamp",
                    new[] { "2014-09-24T10:59:41Z" }
                },
                {
                    "content-type",
                    new[] { "application/json" }
                },
                {
                    "authorization",
                    new[]
                    {
                        "application 669E367E-6BBA-48AB-AF15-266871C28135:Tg6fMyo8mj9pYfWQ9ssbx3Tc1BNC87IEygAfLbJqZb4="
                    }
                }
            };

            const string body =
                "{\"event\":\"ace\",\"callid\":\"822aa4b7-05b4-4d83-87c7-1f835ee0b6f6_257\",\"timestamp\":\"2014-09-24T10:59:41Z\",\"version\":1}";

            var result = sinchEvents.ValidateAuthenticationHeader(HttpMethod.Post, "/sinch/callback/ace", headers, body);

            result.Should().BeTrue();
        }

        [Fact]
        public void SerializeResponse_SerializesDerivedVerificationResponse()
        {
            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));
            var response = new SmsRequestEventResponse
            {
                Action = Action.Allow,
                Sms = new Sms
                {
                    Code = "123",
                    AcceptLanguage = new List<string> { "en-US" }
                }
            };

            var json = sinchEvents.SerializeResponse(response);

            json.Should().Contain("\"action\":\"allow\"");
            json.Should().Contain("\"sms\"");
            json.Should().Contain("\"code\":\"123\"");
        }
    }
}