using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification.Common;
using Sinch.Verification.Report.Response;
using Xunit;

namespace Sinch.Tests.e2e.Verification
{
    public class VerificationStatusTests : VerificationTestBase
    {
        // mocked based on oas file
        private readonly ReportSmsVerificationResponse _smsVerificationReportResponse =
            new ReportSmsVerificationResponse()
            {
                Id = "some_string_value",
                Method = VerificationMethod.Sms,
                Price = new PriceBase
                {
                    VerificationPrice = new()
                    {
                        CurrencyId = "some_string_value",
                        Amount = 1.1
                    }
                },
                Reason = new Reason("some_string_value"),
                Reference = "some_string_value",
                Source = new Source("some_string_value"),
                Status = new VerificationStatus("some_string_value")
            };

        [Fact]
        public async Task StatusById()
        {
            var response = await VerificationClient.VerificationStatus.GetSmsById("123");

            response.Should().BeOfType<ReportSmsVerificationResponse>().Which.Should().BeEquivalentTo(
                _smsVerificationReportResponse);
        }

        [Fact]
        public async Task StatusByIdentity()
        {
            var response =
                await VerificationClient.VerificationStatus.GetSmsByIdentity("123");

            response.Should()
                .BeEquivalentTo(_smsVerificationReportResponse);
        }

        [Fact]
        public async Task StatusByReference()
        {
            var response = await VerificationClient.VerificationStatus.GetSmsByReference("123");

            response.Should().BeOfType<ReportSmsVerificationResponse>().Which.Should()
                .BeEquivalentTo(_smsVerificationReportResponse);
        }

        [Fact]
        public async Task ByReferenceSms()
        {
            var response = await VerificationClient.VerificationStatus.GetSmsById("12");

            response.Should().BeEquivalentTo(
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
        public async Task ByIdentityPhoneCall()
        {
            var response =
                await VerificationClient.VerificationStatus.GetCalloutByIdentity("+49342432");

            response.Should().BeEquivalentTo(
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
        public async Task ByReferenceFlashCall()
        {
            var response =
                await VerificationClient.VerificationStatus.GetFlashcallByReference("ref_12");

            response.Should().BeEquivalentTo(
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
    }
}
