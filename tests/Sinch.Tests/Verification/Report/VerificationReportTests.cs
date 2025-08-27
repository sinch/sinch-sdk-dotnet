using System.Text.Json;
using FluentAssertions;
using Sinch.Verification.Common;
using Sinch.Verification.Report.Request;
using Sinch.Verification.Report.Response;
using Xunit;

namespace Sinch.Tests.Verification
{
    public class VerificationReportTests
    {
        public static ReportWhatsAppVerificationRequest reportWhatsAppVerificationRequest = new ReportWhatsAppVerificationRequest()
        {
            WhatsApp = new Sinch.Verification.Report.Request.WhatsApp()
            {
                Code = "foo code",
            },
        };

        public static ReportWhatsAppVerificationResponse reportWhatsAppVerificationResponse = new ReportWhatsAppVerificationResponse()
        {
            Id = "the id",
            Status = VerificationStatus.Fail,
            Reason = Reason.Expired
        };

        private JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };


        [Fact]
        public void SerializeVerificationReportWhatsAppRequest()
        {
            var expected = Helpers.LoadResources("Verification/Report/VerificationReportRequestWhatsAppDto.json");

            var responseJson = JsonSerializer.Serialize(reportWhatsAppVerificationRequest);
            Helpers.AssertJsonEqual(expected, responseJson);
        }

        [Fact]
        public void DeSerializeVerificationReportWhatsAppResponse()
        {
            var data = Helpers.LoadResources("Verification/Report/VerificationReportResponseWhatsAppDto.json");
            var response = JsonSerializer.Deserialize<IVerificationReportResponse>(data, _jsonSerializerOptions);

            var actual = response.Should().BeOfType<ReportWhatsAppVerificationResponse>().Subject;
            Helpers.BeEquivalentToWithJsonElement(actual, reportWhatsAppVerificationResponse);
        }
    }
}
