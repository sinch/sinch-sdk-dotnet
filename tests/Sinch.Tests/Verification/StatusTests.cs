using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;

/* Unmerged change from project 'Sinch.Tests(net7.0)'
Before:
using Sinch.Verification.Report.Response;
using Sinch.Verification.Status;
After:
using Sinch.Verification.Status;
*/
using Sinch.Verification.Status;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class StatusTests : VerificationTestBase
    {
        [Fact]
        public async Task GetWhatsAppById()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://verification.api.sinch.com/verification/v1/verifications/id/the-id")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond("application/json", Helpers.LoadResources("Verification/Status/VerificationStatusResponseWhatsAppDto.json"));

            var response = await VerificationStatus.GetWhatsAppById("the-id");

            response.Should().BeOfType<WhatsAppVerificationStatusResponse>();
            Helpers.BeEquivalentToWithJsonElement(response, VerificationStatusTests.whatsAppVerificationStatusResponse);
        }

        [Fact]
        public async Task GetWhatsAppByIdentity()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://verification.api.sinch.com/verification/v1/verifications/whatsapp/number/+33123456789")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond("application/json", Helpers.LoadResources("Verification/Status/VerificationStatusResponseWhatsAppDto.json"));

            var response = await VerificationStatus.GetWhatsAppByIdentity("+33123456789");

            response.Should().BeOfType<WhatsAppVerificationStatusResponse>();
            Helpers.BeEquivalentToWithJsonElement(response, VerificationStatusTests.whatsAppVerificationStatusResponse);
        }

        [Fact]
        public async Task GetWhatsAppByReference()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Get, $"https://verification.api.sinch.com/verification/v1/verifications/reference/%F0%9F%90%9D")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .Respond("application/json", Helpers.LoadResources("Verification/Status/VerificationStatusResponseWhatsAppDto.json"));

            var response = await VerificationStatus.GetWhatsAppByReference("🐝");

            response.Should().BeOfType<WhatsAppVerificationStatusResponse>();
            Helpers.BeEquivalentToWithJsonElement(response, VerificationStatusTests.whatsAppVerificationStatusResponse);
        }
    }
}
