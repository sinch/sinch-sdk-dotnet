using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Sinch.Verification.Start;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationTests
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
                _links = new
                {
                    rel = "status",
                    href = "string",
                    method = "GET"
                }
            };

            var jData = JToken.FromObject(data).ToString();
            _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var smsResponse = JsonSerializer.Deserialize<IVerificationResponse>(jData, _jsonSerializerOptions);

            smsResponse.Should().BeOfType<SmsResponse>().Which.Should().BeEquivalentTo(new SmsResponse()
            {
                Id = "1234567890",
                Method = "sms",
                Sms = new SmsInfo()
                {
                    Template = "Your verification code is {{CODE}}",
                    InterceptionTimeout = 32
                },
                Links = new Links()
                {
                    Rel = "status",
                    Href = "string",
                    Method = "GET"
                }
            });
        }
    }
}
