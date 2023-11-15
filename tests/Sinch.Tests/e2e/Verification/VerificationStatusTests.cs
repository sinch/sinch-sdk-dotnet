using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification.Common;
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
                Method = VerificationMethod.Sms,
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
            var response =
                await VerificationClient.VerificationStatus.GetByIdentity("123",
                    VerificationMethod.Sms);

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

        [Fact]
        public async Task ByReferenceSms()
        {
            var response = await VerificationClient.VerificationStatus.GetById("12");

            response.Should().BeOfType<SmsVerificationReportResponse>().Which.Should().BeEquivalentTo(
                new SmsVerificationReportResponse()
                {
                    Method = VerificationMethod.Sms,
                    Reference = "ref",
                    Id = "_id",
                    Price = new PriceBase()
                    {
                        VerificationPrice = new PriceDetail()
                        {
                            Amount = 0.42,
                            CurrencyId = "US"
                        }
                    },
                    Reason = Reason.DeniedByCallback,
                    Source = Source.Intercepted,
                    Status = VerificationStatus.Aborted
                });
        }

        [Fact]
        public async Task ByIdentityPhoneCall()
        {
            var response =
                await VerificationClient.VerificationStatus.GetByIdentity("+49342432",
                    VerificationMethod.Callout);

            response.Should().BeOfType<PhoneCallVerificationReportResponse>().Which.Should().BeEquivalentTo(
                new PhoneCallVerificationReportResponse()
                {
                    Method = VerificationMethod.Callout,
                    Id = "_id",
                    Price = new Price()
                    {
                        VerificationPrice = new PriceDetail()
                        {
                            Amount = 0.42,
                            CurrencyId = "EUR"
                        },
                        TerminationPrice = new PriceDetail()
                        {
                            Amount = 0.11,
                            CurrencyId = "EUR"
                        },
                        BillableDuration = 40
                    },
                    Status = VerificationStatus.Error,
                    Reason = Reason.NetworkErrorOrUnreachable,
                    CallComplete = true,
                });
        }
        
        [Fact]
        public async Task ByReferenceFlashCall()
        {
            var response =
                await VerificationClient.VerificationStatus.GetByReference("ref_12");

            response.Should().BeOfType<FlashCallVerificationReportResponse>().Which.Should().BeEquivalentTo(
                new FlashCallVerificationReportResponse()
                {
                    Method = VerificationMethod.FlashCall,
                    Id = "_id",
                    Price = new Price()
                    {
                        VerificationPrice = new PriceDetail()
                        {
                            Amount = 0.42,
                            CurrencyId = "EUR"
                        },
                        TerminationPrice = new PriceDetail()
                        {
                            Amount = 0.11,
                            CurrencyId = "EUR"
                        },
                        BillableDuration = 40
                    },
                    Status = VerificationStatus.Successful
                });
        }
    }
}
