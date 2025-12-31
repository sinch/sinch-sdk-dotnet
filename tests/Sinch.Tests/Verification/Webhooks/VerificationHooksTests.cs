using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Sinch.Verification.Common;
using Sinch.Verification.Hooks;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationHooksTests
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
        public void SerializeHookResponse()
        {
            var response = new SmsRequestEventResponse
            {
                Action = Action.Allow,
                Sms = new Sinch.Verification.Hooks.Sms
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
        public void SerializeHookWhatsAppResponse()
        {
            var expected = Helpers.LoadResources("Verification/Webhooks/VerificationResponseWhatsAppDto.json");

            var response = new WhatsAppRequestEventResponse
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
