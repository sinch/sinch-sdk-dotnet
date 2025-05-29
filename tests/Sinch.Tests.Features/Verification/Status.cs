using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.Status;

namespace Sinch.Tests.Features.Verification
{
    [Binding]
    public class Status
    {
        private ISinchVerificationStatus _sinchVerificationStatus;
        private SmsVerificationStatusResponse _smsVerification;
        private CalloutVerificationStatusResponse _phoneVerificationStatus;
        private FlashCallVerificationStatusResponse _flashcallStatus;

        [Given(@"the Verification service ""Status"" is available")]
        public void GivenTheVerificationServiceIsAvailable()
        {
            _sinchVerificationStatus = Utils.SinchVerificationClient.VerificationStatus;
            _sinchVerificationStatus.Should().NotBeNull();
        }

        [When(@"I send a request to retrieve a SMS verification status by its verification ID")]
        public async Task WhenISendARequestToRetrieveAsmsVerificationStatusByItsVerificationId()
        {
            _smsVerification = await _sinchVerificationStatus.GetSmsById("1ce0ffee-c0de-5eed-d00d-f00dfeed1337");
        }

        [Then(@"the response contains the details of the SMS verification status")]
        public void ThenTheResponseContainsTheDetailsOfTheSmsVerificationStatus()
        {
            _smsVerification.Should().BeEquivalentTo(new SmsVerificationStatusResponse
            {
                Id = "1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                Status = VerificationStatus.Successful,
                CountryId = "FR",
                VerificationTimestamp = Helpers.ParseUtc("2024-06-06T09:08:41.4784877Z"),
                Identity = Identity.Number("+33612345678"),
                Price = new PriceBase
                {
                    VerificationPrice = new PriceDetail()
                    {
                        Amount = 0.0453,
                        CurrencyId = "EUR"
                    },
                }
            });
        }

        [When(@"I send a request to retrieve a Phone Call verification status by the phone number to verify")]
        public async Task WhenISendARequestToRetrieveAPhoneCallVerificationStatusByThePhoneNumberToVerify()
        {
            _phoneVerificationStatus = await _sinchVerificationStatus.GetCalloutByIdentity("+33612345678");
        }

        [Then(@"the response contains the details of the Phone Call verification status")]
        public void ThenTheResponseContainsTheDetailsOfThePhoneCallVerificationStatus()
        {
            _phoneVerificationStatus.Should().BeEquivalentTo(new CalloutVerificationStatusResponse
            {
                Id = "1ce0ffee-c0de-5eed-d11d-f00dfeed1337",
                Status = VerificationStatus.Successful,
                CountryId = "FR",
                VerificationTimestamp = Helpers.ParseUtc("2024-06-06T09:10:27.7264837Z"),
                Identity = Identity.Number("+33612345678"),
                CallComplete = true,
                Price = new Price()
                {
                    BillableDuration = 0,
                    TerminationPrice = new PriceDetail()
                    {
                        Amount = 0,
                        CurrencyId = "EUR"
                    },
                    VerificationPrice = new PriceDetail()
                    {
                        Amount = 0.1852d,
                        CurrencyId = "EUR"
                    }
                }
            });
        }

        [When(@"I send a request to retrieve a Flash Call verification status by its reference")]
        public async Task WhenISendARequestToRetrieveAFlashCallVerificationStatusByItsReference()
        {
            _flashcallStatus =
                await _sinchVerificationStatus.GetFlashcallByReference("flashcall-verification-test-e2e");
        }

        [Then(@"the response contains the details of the Flash Call verification status")]
        public void ThenTheResponseContainsTheDetailsOfTheFlashCallVerificationStatus()
        {
            _flashcallStatus.Should().BeEquivalentTo(new FlashCallVerificationStatusResponse
            {
                Id = "1ce0ffee-c0de-5eed-d22d-f00dfeed1337",
                Status = VerificationStatus.Successful,
                Reference = "flashcall-verification-test-e2e",
                CountryId = "FR",
                VerificationTimestamp = Helpers.ParseUtc("2024-06-06T09:07:32.3554646Z"),
                Identity = Identity.Number("+33612345678"),
                Price = new Price()
                {
                    BillableDuration = 0,
                    TerminationPrice = new PriceDetail()
                    {
                        Amount = 0,
                        CurrencyId = "EUR"
                    },
                    VerificationPrice = new PriceDetail()
                    {
                        Amount = 0.0205d,
                        CurrencyId = "EUR"
                    }
                }
            });
        }
    }
}
