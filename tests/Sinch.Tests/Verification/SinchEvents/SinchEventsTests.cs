using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using FluentAssertions;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.SinchEvents;
using Xunit;

namespace Sinch.Tests.Verification.SinchEvents
{
    public class SinchEventsTests
    {
        [Fact]
        public void ParseEvent_ReturnsVerificationRequestEvent()
        {
            var json = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEvent.json");

            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var parsed = sinchEvents.ParseEvent(json);

            parsed.Should().BeOfType<VerificationStartEvent>()
                .Which.Id.Should().Be("1234567890");
        }

        [Fact]
        public void ParseEvent_ReturnsVerificationResultEvent()
        {
            var json = Helpers.LoadResources("Verification/SinchEvents/VerificationResultEvent.json");

            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var parsed = sinchEvents.ParseEvent(json);

            parsed.Should().BeOfType<VerificationResultEvent>()
                .Which.Status.Should().Be(VerificationStatus.Successful);
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

            System.Action act = () => sinchEvents.ParseEvent(json);

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
        public void ValidateAuthenticationHeader_Throws_WhenVerificationCredentialsAreUnavailable()
        {
            var sinchEvents = new SinchClient().Verification.SinchEvents;

            System.Action act = () => sinchEvents.ValidateAuthenticationHeader(
                HttpMethod.Post,
                "/sinch/callback/ace",
                new Dictionary<string, IEnumerable<string>>(),
                "{}");

            act.Should().Throw<System.InvalidOperationException>()
                .WithMessage("Verification application credentials are required to validate the authentication header.");
        }

        [Fact]
        public void SerializeResponse_SerializesDerivedVerificationResponse()
        {
            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));
            var response = new VerificationStartEventResponseSms
            {
                Action = Sinch.Verification.SinchEvents.Action.Allow,
                Sms = new Sinch.Verification.SinchEvents.Sms
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

        [Fact]
        public void SerializeResponse_MatchesExpectedSmsPayload()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseSms.json");

            var response = new VerificationStartEventResponseSms
            {
                Action = Action.Allow,
                Sms = new Sinch.Verification.SinchEvents.Sms
                {
                    Code = "123",
                    AcceptLanguage = new List<string>()
                    {
                        "en-US"
                    }
                }
            };

            var json = JsonSerializer.Serialize(response);

            Helpers.AssertJsonEqual(expected, json);
        }

        [Fact]
        public void DeserializeVerificationRequestEvent_ReturnsExpectedEvent()
        {
            var jsonString = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEvent.json");

            var deserialized = JsonSerializer.Deserialize<VerificationStartEvent>(jsonString);

            deserialized.Should().BeEquivalentTo(new VerificationStartEvent()
            {
                Id = "1234567890",
                Event = "VerificationStartEvent",
                Method = VerificationMethod.Sms,
                Identity = new Identity()
                {
                    Endpoint = "+11235551234",
                    Type = IdentityType.Number,
                },
                Price = new PriceDetail()
                {
                    Amount = 10.5,
                    CurrencyId = "USD",
                },
                Reference = "string",
                Custom = "string",
                AcceptLanguage = new List<string>()
                {
                    "es-ES"
                }
            });
        }

        [Fact]
        public void DeserializeVerificationResultEvent_ReturnsExpectedEvent()
        {
            var jsonString = Helpers.LoadResources("Verification/SinchEvents/VerificationResultEvent.json");

            var deserialized = JsonSerializer.Deserialize<VerificationResultEvent>(jsonString);

            deserialized.Should().BeEquivalentTo(new VerificationResultEvent()
            {
                Id = "1234567890",
                Event = "VerificationResultEvent",
                Method = VerificationMethodEx.Sms,
                Identity = new Identity()
                {
                    Endpoint = "+11235551234",
                    Type = IdentityType.Number,
                },
                Reference = "12345",
                Custom = "string",
                Reason = Reason.Fraud,
                Source = Source.Intercepted,
                Status = VerificationStatus.Successful
            });
        }

        [Fact]
        public void SerializeResponse_ReturnsExpectedSmsPayload_WhenSmsResponseProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseSms.json");

            var response = new VerificationStartEventResponseSms
            {
                Action = Action.Allow,
                Sms = new Sinch.Verification.SinchEvents.Sms
                {
                    Code = "123",
                    AcceptLanguage = new List<string>()
                    {
                        "en-US"
                    }
                }
            };

            var json = JsonSerializer.Serialize(response);

            Helpers.AssertJsonEqual(expected, json);
        }

        [Fact]
        public void SerializeResponse_ReturnsExpectedWhatsAppPayload_WhenWhatsAppResponseProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseWhatsApp.json");

            var response = new VerificationStartEventResponseWhatsApp
            {
                Action = Action.Allow,
                WhatsApp = new WhatsApp
                {
                    CodeType = WhatsAppCodeType.Numeric,
                    AcceptLanguage = new List<string>()
                    {
                        "a language"
                    },
                    AdditionalProperties = new Dictionary<string, JsonElement>()
                    {
                        { "my key", JsonDocument.Parse("\"my value\"").RootElement }
                    }
                }
            };

            var responseJson = JsonSerializer.Serialize(response);
            Helpers.AssertJsonEqual(expected, responseJson);
        }
    }
}
