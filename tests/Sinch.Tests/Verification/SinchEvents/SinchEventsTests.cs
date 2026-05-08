using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.SinchEvents;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class SinchEventsTests
    {
        [Fact]
        public void ShouldDeserializeVerificationRequestEvent()
        {
            string jsonString = @"
            {
                ""id"": ""1234567890"",
                ""event"": ""VerificationRequestEvent"",
                ""method"": ""sms"",
                ""identity"": {
                    ""type"": ""number"",
                    ""endpoint"": ""+11235551234""
                },
                ""price"": {
                    ""amount"": 10.5,
                    ""currencyId"": ""USD""
                },
                ""reference"": ""string"",
                ""custom"": ""string"",
                ""acceptLanguage"": [
                    ""es-ES""
                ]
            }";

            var deserialized = JsonSerializer.Deserialize<VerificationRequestEvent>(jsonString);

            deserialized.Should().BeEquivalentTo(new VerificationRequestEvent()
            {
                Id = "1234567890",
                Event = "VerificationRequestEvent",
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
        public void ShouldDeserializeVerificationResultEvent()
        {
            string jsonString = @"
            {
            ""id"": ""1234567890"",
            ""event"": ""VerificationResultEvent"",
            ""method"": ""sms"",
            ""identity"": {
                ""type"": ""number"",
                ""endpoint"": ""+11235551234""
            },
            ""status"": ""PENDING"",
            ""reason"": ""Fraud"",
            ""reference"": ""12345"",
            ""source"": ""intercepted"",
            ""custom"": ""string""
            }";

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
                Status = VerificationStatus.Pending
            });
        }

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
                .Which.Status.Should().Be(Sinch.Verification.Common.VerificationStatus.Successful);
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
        public void SerializeResponse_SerializesDerivedVerificationResponse()
        {
            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));
            var response = new SmsRequestEventResponse
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
        public void SerializeHookWhatsAppResponse()
        {
            var expected = Helpers.LoadResources("Verification/Webhooks/VerificationResponseWhatsAppDto.json");

            var response = new WhatsAppRequestEventResponse
            {
                Action = Sinch.Verification.SinchEvents.Action.Allow,
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

        [Fact]
        public void SerializeResponse_MatchesExpectedSmsPayload()
        {
            var response = new SmsRequestEventResponse
            {
                Action = Sinch.Verification.SinchEvents.Action.Allow,
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

            var expected = JToken.Parse(@"
                                {
                                    ""action"": ""allow"",
                                    ""sms"": {
                                        ""code"": ""123"",
                                        ""acceptLanguage"": [""en-US""]
                                     }
                                }");
            var actual = JToken.Parse(json);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}