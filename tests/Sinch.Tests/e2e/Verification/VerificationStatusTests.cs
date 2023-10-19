using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification.Report.Response;
using Xunit;

namespace Sinch.Tests.e2e.Verification
{
    public class VerificationStatusTests : VerificationTestBase
    {
        [Fact]
        public async Task StatusById()
        {
            var response = await VerificationClient.VerificationStatus.GetById("123");

            response.Should().BeOfType<SmsVerificationReportResponse>().Which.Should().BeEquivalentTo(
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
                });
        }
    }
}
