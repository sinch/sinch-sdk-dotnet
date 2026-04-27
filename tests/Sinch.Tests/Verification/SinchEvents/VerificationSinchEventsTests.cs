using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Sinch.Verification.Common;
using Sinch.Verification.SinchEvents;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationSinchEventsTests
    {
        [Fact]
        public void ShouldDeserializeVerificationRequestEvent()
        {
            string jsonString = @"
            {
                ""id"": ""1234567890"",
                ""event"": ""VerificationSinchEventRequest"",
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

            var deserialized = JsonSerializer.Deserialize<VerificationStartEvent>(jsonString);

            deserialized.Should().BeEquivalentTo(new VerificationStartEvent()
            {
                Id = "1234567890",
                Event = "VerificationSinchEventRequest",
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
            ""event"": ""VerificationSinchEventResult"",
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
                Event = "VerificationSinchEventResult",
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
        public void SerializeSinchEventResponse()
        {
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

        [Fact]
        public void SerializeWhatsAppSinchEventResponse()
        {
            var expected = Helpers.LoadResources("Verification/SinchEvents/VerificationResponseWhatsAppDto.json");

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
