using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Response;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationTests : VerificationTestBase
    {
        private JsonSerializerOptions _jsonSerializerOptions;

        [Fact]
        public void DeserializeVerificationStartResponse()
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
            _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
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
    }
}
