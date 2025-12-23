using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Verification.Start.Response;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class StartTests : VerificationTestBase
    {
        [Fact]
        public async Task StartWhatsApp()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Post, $"https://verification.api.sinch.com/verification/v1/verifications")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Verification/Start/VerificationStartRequestWhatsAppDto.json"))
                .Respond("application/json", Helpers.LoadResources("Verification/Start/VerificationStartResponseWhatsAppDto.json"));

            var response = await Verification.StartWhatsApp(VerificationStartTests.startWhatsAppVerificationRequest);

            response.Should().BeOfType<StartWhatsAppVerificationResponse>();
            Helpers.BeEquivalentToWithJsonElement(response, VerificationStartTests.startWhatsAppVerificationResponse);
        }
    }
}
