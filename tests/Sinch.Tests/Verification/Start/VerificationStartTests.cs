using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;
using Sinch.Verification.Start.Response;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationStartTests
    {

        static public StartWhatsAppVerificationRequest startWhatsAppVerificationRequest = new StartWhatsAppVerificationRequest()
        {
            Identity = Identity.Number("+33123456789"),
            Reference = "a reference",
            Custom = "a custom",
            WhatsAppInfo = new Sinch.Verification.Start.Request.WhatsAppInfo()
            {
                CodeType = WhatsAppCodeType.Alphanumeric,
                AdditionalProperties = new Dictionary<string, JsonElement>()
                        {
                            { "my key", JsonDocument.Parse("\"my value\"").RootElement }
                        }
            },
        };

        static public StartWhatsAppVerificationResponse startWhatsAppVerificationResponse = new StartWhatsAppVerificationResponse()
        {
            Id = "the id",
            WhatsApp = new Sinch.Verification.Start.Response.WhatsAppInfo
            {
                CodeType = WhatsAppCodeType.Numeric,
                AdditionalProperties = new Dictionary<string, JsonElement>()
                    {
                            { "my key", JsonDocument.Parse("\"my value\"").RootElement }
                    }
            },
            Links = new List<Links>
                {
                    new Links
                    {
                        Rel = "status",
                        Href = "an href for status",
                        Method = "GET"
                    }, new Links
                    {
                        Rel = "report",
                        Href = "an href for report",
                        Method = "PUT"
                    }
                }
        };

        private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        [Fact]
        public void DeserializeVerificationStartSmsResponse()
        {
            var data = new
            {
                id = "1234567890",
                method = "sms",
                sms = new
                {
                    template = "Your verification code is {{CODE}}",
                    interceptionTimeout = 32
                },
                _links = new[]
                {
                    new
                    {
                        rel = "status",
                        href = "string",
                        method = "GET"
                    }
                }
            };

            var jData = JToken.FromObject(data).ToString();
            var smsResponse = JsonSerializer.Deserialize<IStartVerificationResponse>(jData, _jsonSerializerOptions);

            smsResponse.Should().BeOfType<StartSmsVerificationResponse>().Which.Should().BeEquivalentTo(
                new StartSmsVerificationResponse()
                {
                    Id = "1234567890",
                    Method = VerificationMethodEx.Sms,
                    Sms = new SmsInfo()
                    {
                        Template = "Your verification code is {{CODE}}",
                        InterceptionTimeout = 32
                    },
                    Links = new List<Links>()
                    {
                        new()
                        {
                            Rel = "status",
                            Href = "string",
                            Method = "GET"
                        }
                    }
                });
        }

        [Fact]
        public void SerializeVerificationStartWhatsAppRequest()
        {
            var expected = Helpers.LoadResources("Verification/Start/VerificationStartRequestWhatsAppDto.json");

            var responseJson = JsonSerializer.Serialize(startWhatsAppVerificationRequest);
            Helpers.AssertJsonEqual(expected, responseJson);

        }

        [Fact]
        public void DeSerializeVerificationStartWhatsAppResponse()
        {
            var data = Helpers.LoadResources("Verification/Start/VerificationStartResponseWhatsAppDto.json");
            var response = JsonSerializer.Deserialize<IStartVerificationResponse>(data, _jsonSerializerOptions);

            var actual = response.Should().BeOfType<StartWhatsAppVerificationResponse>().Subject;
            Helpers.BeEquivalentToWithJsonElement(actual, startWhatsAppVerificationResponse);
        }
    }
}
