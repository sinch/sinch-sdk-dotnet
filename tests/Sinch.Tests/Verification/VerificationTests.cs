using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using Sinch.Verification.Common;
using Sinch.Verification.Report.Request;
using Sinch.Verification.Report.Response;
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

        [Fact]
        public async Task VerifyReport()
        {
            const string endpoint = "+44000000000";
            HttpMessageHandlerMock.When(HttpMethod.Put,
                    $"https://verification.api.sinch.com/verification/v1/verifications/number/{endpoint}")
                .Respond(JsonContent.Create(new
                {
                    id = "10",
                    callComplete = true,
                    method = "callout"
                }));

            var response = await VerificationClient.Verification.ReportPhoneCallByIdentity(endpoint,
                new ReportPhoneCallVerificationRequest
                {
                    Callout = new Callout
                    {
                        Code = "1"
                    }
                });

            response.Should().BeOfType<ReportPhoneCallVerificationResponse>().Which.Should().BeEquivalentTo(
                new ReportPhoneCallVerificationResponse
                {
                    Id = "10",
                    CallComplete = true,
                    Method = VerificationMethod.Callout
                });
        }
    }
}
