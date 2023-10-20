using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification.Report;
using Sinch.Verification.Report.Response;
using Sinch.Verification.Start.Request;
using Xunit;

namespace Sinch.Tests.e2e.Verification
{
    public class ReportVerificationTests : VerificationTestBase
    {
        [Fact]
        public async Task ReportSmsByIdentity()
        {
            var response = await VerificationClient.Verification.ReportIdentity("+48123123",
                new SmsVerificationReportRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Cli = "cli",
                        Code = "228"
                    }
                });

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
        public async Task ReportFlashCallByIdentity()
        {
            var response = await VerificationClient.Verification.ReportIdentity("+48123123",
                new FlashCallVerificationReportRequest()
                {
                    FlashCall = new FlashCall()
                    {
                        Cli = "cli"
                    },
                });

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

        [Fact]
        public async Task ReportPhoneCallByIdentity()
        {
            var response = await VerificationClient.Verification.ReportIdentity("+48123123",
                new PhoneCallVerificationReportRequest
                {
                    Callout = new Callout()
                    {
                        Code = "13",
                    }
                });

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
        public async Task ReportSmsById()
        {
            var response = await VerificationClient.Verification.ReportId("123",
                new SmsVerificationReportRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Code = "13"
                    }
                });

            response.Should().BeOfType<SmsVerificationReportResponse>().Which.Should().BeEquivalentTo(
                new SmsVerificationReportResponse()
                {
                    Method = VerificationMethod.Sms,
                    Id = "_id",
                    Price = new PriceBase()
                    {
                        VerificationPrice = new PriceDetail()
                        {
                            Amount = 0.42,
                            CurrencyId = "USD"
                        },
                    },
                    Status = VerificationStatus.Aborted,
                    Reason = Reason.DeniedByCallback,
                    Source = Source.Manual,
                    Reference = "ref",
                });
        }
    }
}
