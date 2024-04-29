using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification.Common;
using Sinch.Verification.Report.Request;
using Sinch.Verification.Report.Response;
using Xunit;

namespace Sinch.Tests.e2e.Verification
{
    public class ReportVerificationTests : VerificationTestBase
    {
        [Fact]
        public async Task ReportSmsByIdentity()
        {
            var response = await VerificationClient.Verification.ReportSmsByIdentity("+48123123",
                new ReportSmsVerificationRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Cli = "cli",
                        Code = "228"
                    }
                });

            response.Should().BeEquivalentTo(
                new ReportSmsVerificationResponse()
                {
                    Id = "123456",
                    Status = VerificationStatus.Successful,
                });
        }

        [Fact]
        public async Task ReportFlashCallByIdentity()
        {
            var response = await VerificationClient.Verification.ReportFlashCallByIdentity("+48123123",
                new ReportFlashCallVerificationRequest()
                {
                    FlashCall = new FlashCall()
                    {
                        Cli = "cli"
                    },
                });

            response.Should().BeEquivalentTo(
                new ReportFlashCallVerificationResponse()
                {
                    Id = "123456",
                    Status = VerificationStatus.Fail
                });
        }

        [Fact]
        public async Task ReportCalloutByIdentity()
        {
            var response = await VerificationClient.Verification.ReportCalloutByIdentity("+48123123",
                new ReportCalloutVerificationRequest
                {
                    Callout = new Callout()
                    {
                        Code = "13",
                    }
                });

            response.Should().BeEquivalentTo(
                new ReportCalloutVerificationResponse()
                {
                    Id = "123456",
                    Status = VerificationStatus.Aborted,
                });
        }

        [Fact]
        public async Task ReportSmsById()
        {
            var response = await VerificationClient.Verification.ReportSmsById("123",
                new ReportSmsVerificationRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Code = "13"
                    }
                });

            response.Should().BeEquivalentTo(
                new ReportSmsVerificationResponse()
                {
                    Id = "_id",
                    Status = VerificationStatus.Aborted,
                });
        }
    }
}
