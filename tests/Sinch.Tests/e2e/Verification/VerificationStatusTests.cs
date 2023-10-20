using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification;
using Sinch.Verification.Report.Response;
using Xunit;

namespace Sinch.Tests.e2e.Verification
{
    public class VerificationStatusTests : VerificationTestBase
    {
        private readonly SmsVerificationReportResponse _smsVerificationReportResponse =
            new SmsVerificationReportResponse()
            {
                Id = "1234567890",
                Method = "sms",
                Price = new PriceBase
                {
                    VerificationPrice = new()
                    {
                        CurrencyId = "USD",
                        Amount = 0.0127
                    }
                },
                Reason = Reason.Fraud,
                Reference = "12345",
                Source = Source.Intercepted,
                Status = VerificationStatus.Fail
            };

        [Fact]
        public async Task StatusById()
        {
            var response = await VerificationClient.VerificationStatus.GetById("123");

            response.Should().BeOfType<SmsVerificationReportResponse>().Which.Should().BeEquivalentTo(
                _smsVerificationReportResponse);
        }

        [Fact]
        public async Task StatusByIdentity()
        {
            var response = await VerificationClient.VerificationStatus.GetByIdentity("123", VerificationMethod.Sms);

            response.Should().BeOfType<SmsVerificationReportResponse>().Which.Should()
                .BeEquivalentTo(_smsVerificationReportResponse);
        }

        [Fact]
        public async Task StatusByReference()
        {
            var response = await VerificationClient.VerificationStatus.GetByReference("123");

            response.Should().BeOfType<SmsVerificationReportResponse>().Which.Should()
                .BeEquivalentTo(_smsVerificationReportResponse);
        }
    }
}
