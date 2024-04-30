using System;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Verification.Common;
using Sinch.Verification.Status;
using Xunit;

namespace Sinch.Tests.e2e.Verification
{
    public class VerificationStatusTests : VerificationTestBase
    {
        [Fact]
        public async Task ReportStatusByIdSms()
        {
            var response = await VerificationClient.VerificationStatus.GetSmsById("12");

            response.Should().BeEquivalentTo(new SmsVerificationStatusResponse()
            {
                Id = "_id",
                Status = VerificationStatus.Aborted,
                Reason = Reason.DeniedByCallback,
                Reference = "ref",
                Price = new Price()
                {
                    VerificationPrice = new PriceDetail()
                    {
                        CurrencyId = "US",
                        Amount = 0.42d,
                    }
                },
                Source = Source.Intercepted,
                CountryId = "de",
                VerificationTimestamp = new DateTime(2023, 04, 21, 14, 45, 51),
                Identity = new Identity()
                {
                    Type = IdentityType.Number,
                    Endpoint = "+123456"
                },
            });
        }

        [Fact]
        public async Task ByIdentityPhoneCall()
        {
            var response =
                await VerificationClient.VerificationStatus.GetCalloutByIdentity("+49342432");

            response.Should().BeEquivalentTo(
                new CalloutVerificationStatusResponse()
                {
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
                    CountryId = "de",
                    VerificationTimestamp = DateTime.Parse("2023-04-21T14:45:51"),
                    Identity = new Identity()
                    {
                        Type = IdentityType.Number,
                        Endpoint = "+123456"
                    }
                });
        }

        [Fact]
        public async Task ByReferenceFlashCall()
        {
            var response =
                await VerificationClient.VerificationStatus.GetFlashcallByReference("ref_12");

            response.Should().BeEquivalentTo(
                new FlashCallVerificationStatusResponse()
                {
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
                    Status = VerificationStatus.Successful,
                    Identity = new Identity()
                    {
                        Type = IdentityType.Number,
                        Endpoint = "+123456"
                    },
                });
        }
    }
}
