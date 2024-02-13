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

            response.Should().BeOfType<ReportSmsVerificationResponse>().Which.Should().BeEquivalentTo(
                new ReportSmsVerificationResponse()
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
            var response = await VerificationClient.Verification.ReportFlashCallByIdentity("+48123123",
                new ReportFlashCallVerificationRequest()
                {
                    FlashCall = new FlashCall()
                    {
                        Cli = "cli"
                    },
                });

            response.Should().BeOfType<ReportFlashCallVerificationResponse>().Which.Should().BeEquivalentTo(
                new ReportFlashCallVerificationResponse()
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

            response.Should().BeOfType<ReportCalloutVerificationResponse>().Which.Should().BeEquivalentTo(
                new ReportCalloutVerificationResponse()
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
            var response = await VerificationClient.Verification.ReportSmsById("123",
                new ReportSmsVerificationRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Code = "13"
                    }
                });

            response.Should().BeOfType<ReportSmsVerificationResponse>().Which.Should().BeEquivalentTo(
                new ReportSmsVerificationResponse()
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
