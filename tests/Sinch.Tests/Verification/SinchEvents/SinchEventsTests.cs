using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using FluentAssertions;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.SinchEvents;
using Xunit;
using Action = Sinch.Verification.SinchEvents.Action;

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
        public void ParseEvent_ReturnsVerificationSmsDeliveredEvent()
        {
            var json = Helpers.LoadResources("Verification/SinchEvents/VerificationSmsDeliveredEvent.json");

            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var parsed = sinchEvents.ParseEvent(json);

            parsed.Should().BeOfType<VerificationSmsDeliveredEvent>()
                .Which.SmsResult.Should().Be(SmsDeliveryResult.Successful);
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
                .WithMessage("VerificationConfiguration with AppKey and AppSecret is required to use Verification API methods. Set VerificationConfiguration when creating SinchClient.");
        }

        [Fact]
        public void DeserializeVerificationRequestEvent_ReturnsExpectedEvent()
        {
            var jsonString = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEvent.json");
            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var deserialized = sinchEvents.ParseEvent(jsonString);

            deserialized.Should().BeEquivalentTo(new VerificationStartEvent()
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
                Custom = "string"
            });
        }

        [Fact]
        public void DeserializeVerificationResultEvent_ReturnsExpectedEvent()
        {
            var jsonString = Helpers.LoadResources("Verification/SinchEvents/VerificationResultEvent.json");
            var sinchEvents = new VerificationSinchEvents(new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var deserialized = sinchEvents.ParseEvent(jsonString);

            deserialized.Should().BeEquivalentTo(new VerificationResultEvent()
            {
                Id = "1234567890",
                Event = "VerificationResultEvent",
                Method = VerificationMethod.Sms,
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
        public void SerializeResponse_ReturnsExpectedSmsPayload_WhenPartialSmsFieldsProvided()
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
        public void SerializeResponse_ReturnsExpectedWhatsAppPayload_WhenPartialWhatsAppFieldsProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseWhatsApp.json");

            var response = new VerificationStartEventResponseWhatsApp
            {
                Action = Action.Allow,
                WhatsApp = new WhatsApp
                {
                    CodeType = WhatsAppCodeType.Numeric
                }
            };

            var json = JsonSerializer.Serialize(response);

            Helpers.AssertJsonEqual(expected, json);
        }

        [Fact]
        public void SerializeResponse_ReturnsExpectedWhatsAppPayload_WhenAllWhatsAppFieldsProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseWhatsAppAllFields.json");

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

        [Fact]
        public void ParseEvent_DoesNotRequireConfiguration()
        {
            var sinch = new SinchClient();
            var json = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEvent.json");

            var sinchEvent = sinch.Verification.SinchEvents.ParseEvent(json);

            sinchEvent.Should().BeOfType<VerificationStartEvent>();
        }

        [Fact]
        public void SerializeResponse_ReturnsExpectedSmsPayload_WhenAllSmsFieldsProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseSmsAllFields.json");

            var response = new VerificationStartEventResponseSms
            {
                Action = Action.Allow,
                Sms = new Sinch.Verification.SinchEvents.Sms
                {
                    Code = "5666",
                    CodeType = SmsCodeType.Numeric,
                    Expiry = "01:02:03",
                    AcceptLanguage = new List<string> { "fr-FR" },
                    AdditionalProperties = new Dictionary<string, JsonElement>
                    {
                        { "my key", JsonDocument.Parse("\"my value\"").RootElement }
                    }
                }
            };

            var json = JsonSerializer.Serialize(response);

            Helpers.AssertJsonEqual(expected, json);
        }

        [Fact]
        public void SerializeResponse_ReturnsExpectedFlashCallPayload_WhenPartialFlashCallFieldsProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseFlashCall.json");

            var response = new VerificationStartEventResponseFlashCall
            {
                Action = Action.Allow,
                FlashCall = new FlashCall
                {
                    Cli = "+12025550187"
                }
            };

            var json = JsonSerializer.Serialize(response);

            Helpers.AssertJsonEqual(expected, json);
        }

        [Fact]
        public void SerializeResponse_ReturnsExpectedFlashCallPayload_WhenAllFlashCallFieldsProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseFlashCallAllFields.json");

            var response = new VerificationStartEventResponseFlashCall
            {
                Action = Action.Allow,
                FlashCall = new FlashCall
                {
                    Cli = "+12025550187",
                    DialTimeout = 30,
                    InterceptionTimeout = 60,
                    AdditionalProperties = new Dictionary<string, JsonElement>
                    {
                        { "my key", JsonDocument.Parse("\"my value\"").RootElement }
                    }
                }
            };

            var json = JsonSerializer.Serialize(response);

            Helpers.AssertJsonEqual(expected, json);
        }

        [Fact]
        public void SerializeResponse_ReturnsExpectedPhoneCallPayload_WhenPartialPhoneCallFieldsProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponsePhoneCall.json");

            var response = new VerificationStartEventResponsePhoneCall
            {
                Action = Action.Allow,
                PhoneCall = new PhoneCall
                {
                    Code = "1234"
                }
            };

            var json = JsonSerializer.Serialize(response);

            Helpers.AssertJsonEqual(expected, json);
        }

        [Fact]
        public void SerializeResponse_ReturnsExpectedPhoneCallPayload_WhenAllPhoneCallFieldsProvided()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponsePhoneCallAllFields.json");

            var response = new VerificationStartEventResponsePhoneCall
            {
                Action = Action.Allow,
                PhoneCall = new PhoneCall
                {
                    Code = "1234",
                    Speech = new Speech
                    {
                        Locale = "en-US"
                    },
                    AdditionalProperties = new Dictionary<string, JsonElement>
                    {
                        { "my key", JsonDocument.Parse("\"my value\"").RootElement }
                    }
                }
            };

            var json = JsonSerializer.Serialize(response);

            Helpers.AssertJsonEqual(expected, json);
        }

        [Fact]
        public void SerializeResponse_DoesNotRequireConfiguration()
        {
            var sinch = new SinchClient();
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationStartEventResponseSms.json");
            var response = new VerificationStartEventResponseSms
            {
                Action = Action.Allow,
                Sms = new Sinch.Verification.SinchEvents.Sms
                {
                    Code = "123",
                    AcceptLanguage = new List<string> { "en-US" }
                }
            };

            var json = sinch.Verification.SinchEvents.SerializeResponse(response);

            Helpers.AssertJsonEqual(expected, json);
        }
    }
}
