using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Sinch.Verification.Report.Response;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class ReportTests : VerificationTestBase
    {
        [Fact]
        public async Task ReportWhatsAppById()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Put, $"https://verification.api.sinch.com/verification/v1/verifications/id/the-id")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Verification/Report/VerificationReportRequestWhatsAppDto.json"))
                .Respond("application/json", Helpers.LoadResources("Verification/Report/VerificationReportResponseWhatsAppDto.json"));

            var response = await Verification.ReportWhatsAppById("the-id", VerificationReportTests.reportWhatsAppVerificationRequest);

            response.Should().BeOfType<ReportWhatsAppVerificationResponse>();
            Helpers.BeEquivalentToWithJsonElement(response, VerificationReportTests.reportWhatsAppVerificationResponse);
        }


        [Fact]
        public async Task ReportWhatsAppByIdentity()
        {
            HttpMessageHandlerMock
                .When(HttpMethod.Put, $"https://verification.api.sinch.com/verification/v1/verifications/number/+33123456789")
                .WithHeaders("Authorization", $"Bearer {Token}")
                .WithJson(Helpers.LoadResources("Verification/Report/VerificationReportRequestWhatsAppDto.json"))
                .Respond("application/json", Helpers.LoadResources("Verification/Report/VerificationReportResponseWhatsAppDto.json"));

            var response = await Verification.ReportWhatsAppByIdentity("+33123456789", VerificationReportTests.reportWhatsAppVerificationRequest);

            response.Should().BeOfType<ReportWhatsAppVerificationResponse>();
            Helpers.BeEquivalentToWithJsonElement(response, VerificationReportTests.reportWhatsAppVerificationResponse);
        }
    }
}
