using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Specialized;
using Reqnroll;
using Sinch.Numbers;
using Sinch.Numbers.Active;
using Sinch.Numbers.Active.List;
using Sinch.Numbers.Active.Update;
using Sinch.Numbers.Available;
using Sinch.Numbers.Available.List;
using Sinch.Numbers.Available.Rent;
using Sinch.Numbers.Available.RentAny;
using Sinch.Numbers.VoiceConfigurations;

namespace Sinch.Tests.Features.Numbers
{
    [Binding]
    public class NumbersService
    {
        private ISinchNumbers _sinchNumbers;
        private ActiveNumber _updateNumberResponse;
        private ListAvailableNumbersResponse _searchAvailableResponse;
        private Func<Task<AvailableNumber>> _checkAvailabilityResponseOp;
        private ActiveNumber _rentAnyResponse;
        private Func<Task<ActiveNumber>> _rentNumberResponseOp;
        private ListActiveNumbersResponse _listNumbers;
        private IAsyncEnumerable<ActiveNumber> _listAllNumbers;
        private Func<Task<ActiveNumber>> _activeNumberOp;
        private ActiveNumber _releaseResponse;

        [Given(@"the Numbers service is available")]
        public void GivenTheNumbersServiceIsAvailable()
        {
            _sinchNumbers = Utils.SinchNumbersClient();
        }


        [When(@"I send a request to update the phone number ""(.*)""")]
        public async Task WhenISendARequestToUpdateThePhoneNumber(string number)
        {
            _updateNumberResponse = await _sinchNumbers.Update(number, new UpdateActiveNumberRequest()
            {
                DisplayName = "Updated description during E2E tests",
                SmsConfiguration = new SmsConfiguration()
                {
                    ServicePlanId = "SingingMooseSociety"
                },
                VoiceConfiguration = new VoiceFaxConfiguration()
                {
                    ServiceId = "01W4FFL35P4NC4K35FAXSERVICE"
                },
                CallbackUrl = "https://my-callback-server.com/numbers"
            });
        }

        [Then(@"the response contains a phone number with updated parameters")]
        public void ThenTheResponseContainsAPhoneNumberWithUpdatedParameters()
        {
            _updateNumberResponse.Should().BeEquivalentTo(new ActiveNumber()
            {
                ProjectId = "123coffee-dada-beef-cafe-baadc0de5678",
                DisplayName = "Updated description during E2E tests",
                CallbackUrl = "https://my-callback-server.com/numbers",
                PhoneNumber = "+12015555555",
                RegionCode = "US",
                Type = Types.Local,
                Capability = [Product.Sms, Product.Voice],
                Money = new Money()
                {
                    CurrencyCode = "EUR",
                    Amount = 0.80m
                },
                SmsConfiguration = new SmsConfiguration()
                {
                    ServicePlanId = "SpaceMonkeySquadron",
                    CampaignId = string.Empty,
                    ScheduledProvisioning = new ScheduledProvisioning()
                    {
                        ServicePlanId = "SingingMooseSociety",
                        CampaignId = string.Empty,
                        ErrorCodes = [],
                        Status = ProvisioningStatus.Waiting,
                        LastUpdatedTime = Helpers.ParseUtc("2024-06-06T20:02:20.432220Z")
                    }
                },
                VoiceConfiguration = new VoiceRtcConfiguration()
                {
                    AppId = "sunshine-rain-drop-very-beautifulday",
                    ScheduledVoiceProvisioning = new ScheduledVoiceFaxProvisioning()
                    {
                        AppId = string.Empty,
                        ServiceId = "01W4FFL35P4NC4K35FAXSERVICE",
                        Status = ProvisioningStatus.Waiting,
                        LastUpdatedTime = Helpers.ParseUtc("2024-06-06T20:02:20.437509Z")
                    }
                },
                PaymentIntervalMonths = 1,
                NextChargeDate = Helpers.ParseUtc("2024-07-06T20:02:20.118246Z")
            });
        }

        [When(@"I send a request to search for available phone numbers")]
        public async Task WhenISendARequestToSearchForAvailablePhoneNumbers()
        {
            _searchAvailableResponse = await _sinchNumbers.SearchForAvailableNumbers(new ListAvailableNumbersRequest()
            {
                RegionCode = "US",
                Type = Types.Local,
            });
        }

        [Then(@"the response contains ""(.*)"" available phone numbers")]
        public void ThenTheResponseContainsAvailablePhoneNumbers(int p0)
        {
            _searchAvailableResponse.AvailableNumbers.Should().HaveCount(20);
        }

        [Then(@"a phone number contains all the expected properties")]
        public void ThenAPhoneNumberContainsAllTheExpectedProperties()
        {
            _searchAvailableResponse.AvailableNumbers!.First().Should().BeEquivalentTo(new AvailableNumber()
            {
                PhoneNumber = "+12013504948",
                RegionCode = "US",
                Type = Types.Local,
                Capability = [Product.Sms, Product.Voice],
                SetupPrice = new Money()
                {
                    CurrencyCode = "EUR",
                    Amount = 0.80m
                },
                MonthlyPrice = new Money()
                {
                    CurrencyCode = "EUR",
                    Amount = 0.80m
                },
                PaymentIntervalMonths = 1,
                SupportingDocumentationRequired = true,
            });
        }

        [When(@"I send a request to check the availability of the phone number ""(.*)""")]
        public void WhenISendARequestToCheckTheAvailabilityOfThePhoneNumber(string number)
        {
            _checkAvailabilityResponseOp = () => _sinchNumbers.CheckAvailability(number);
        }

        [Then(@"the response displays the phone number ""(.*)"" details")]
        public async Task ThenTheResponseDisplaysThePhoneNumberDetails(string number)
        {
            (await _checkAvailabilityResponseOp()).Should().BeEquivalentTo(new AvailableNumber()
            {
                PhoneNumber = number,
                RegionCode = "US",
                Type = Types.Local,
                Capability = [Product.Sms, Product.Voice],
                SetupPrice = new Money()
                {
                    CurrencyCode = "EUR",
                    Amount = 0.80m
                },
                MonthlyPrice = new Money()
                {
                    CurrencyCode = "EUR",
                    Amount = 0.80m
                },
                PaymentIntervalMonths = 1,
                SupportingDocumentationRequired = true,
            });
        }

        [Then(@"the response contains an error about the number ""(.*)"" not being available")]
        public async Task ThenTheResponseContainsAnErrorAboutTheNumberNotBeingAvailable(string p0)
        {
            ExceptionAssertions<SinchApiException>? ex = null;
            if (_checkAvailabilityResponseOp != null)
            {
                ex = await _checkAvailabilityResponseOp.Should().ThrowAsync<SinchApiException>();
            }

            if (_rentNumberResponseOp != null)
            {
                ex = await _rentNumberResponseOp.Should().ThrowAsync<SinchApiException>();
            }

            if (ex == null)
            {
                throw new InvalidOperationException("All ops where successful");
            }

            ex.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
            ex.Which.Details.First()!["resourceName"]!.AsValue().ToString().Should().Be(p0);
        }

        [When(@"I send a request to rent a number with some criteria")]
        public async Task WhenISendARequestToRentANumberWithSomeCriteria()
        {
            _rentAnyResponse = await _sinchNumbers.RentAny(new RentAnyNumberRequest()
            {
                RegionCode = "US",
                Type = Types.Local,
                Capabilities = [Product.Sms, Product.Voice],
                SmsConfiguration = new SmsConfiguration()
                {
                    ServicePlanId = "SpaceMonkeySquadron",
                },
                VoiceConfiguration = new VoiceRtcConfiguration()
                {
                    AppId = "sunshine-rain-drop-very-beautifulday",
                },
                NumberPattern = new NumberPattern()
                {
                    Pattern = "7654321",
                    SearchPattern = SearchPattern.End
                }
            });
        }

        [Then(@"the response contains a rented phone number")]
        public void ThenTheResponseContainsARentedPhoneNumber()
        {
            _rentAnyResponse.Should().BeEquivalentTo(new ActiveNumber
            {
                PhoneNumber = "+12017654321",
                ProjectId = "123c0ffee-dada-beef-cafe-baadc0de5678",
                DisplayName = "",
                RegionCode = "US",
                Type = Types.Local,
                Capability = new List<Product> { Product.Sms, Product.Voice },
                Money = new Money
                {
                    CurrencyCode = "EUR",
                    Amount = 0.80m
                },
                PaymentIntervalMonths = 1,
                NextChargeDate = Helpers.ParseUtc("2024-06-06T14:42:42.022227Z"),
                ExpireAt = null,
                SmsConfiguration = new SmsConfiguration
                {
                    ServicePlanId = "",
                    ScheduledProvisioning = new ScheduledProvisioning()
                    {
                        ServicePlanId = "SpaceMonkeySquadron",
                        Status = ProvisioningStatus.Waiting,
                        LastUpdatedTime = Helpers.ParseUtc("2024-06-06T14:42:42.596223Z"),
                        CampaignId = "",
                        ErrorCodes = new List<string>()
                    },
                    CampaignId = ""
                },
                VoiceConfiguration = new VoiceRtcConfiguration()
                {
                    AppId = "",
                    ScheduledVoiceProvisioning = new ScheduledVoiceRtcProvisioning()
                    {
                        AppId = "sunshine-rain-drop-very-beautifulday",
                        Status = ProvisioningStatus.Waiting,
                        LastUpdatedTime = Helpers.ParseUtc("2024-06-06T14:42:42.604092Z"),
                    },
                    LastUpdatedTime = null,
                },
                CallbackUrl = ""
            });
        }

        [When(@"I send a request to rent the phone number ""(.*)""")]
        public void WhenISendARequestToRentThePhoneNumber(string number)
        {
            _rentNumberResponseOp = () => _sinchNumbers.Rent(number, new RentActiveNumberRequest()
            {
                SmsConfiguration = new SmsConfiguration()
                {
                    ServicePlanId = "SpaceMonkeySquadron",
                },
                VoiceConfiguration = new VoiceRtcConfiguration()
                {
                    AppId = "sunshine-rain-drop-very-beautifulday"
                }
            });
        }

        [Then(@"the response contains this rented phone number ""(.*)""")]
        public async Task ThenTheResponseContainsThisRentedPhoneNumber(string number)
        {
            (await _rentNumberResponseOp()).PhoneNumber.Should().Be(number);
        }

        [When(@"I send a request to rent the unavailable phone number ""(.*)""")]
        public void WhenISendARequestToRentTheUnavailablePhoneNumber(string p0)
        {
            _rentNumberResponseOp = () => _sinchNumbers.Rent(p0, new RentActiveNumberRequest()
            {
                SmsConfiguration = new SmsConfiguration()
                {
                    ServicePlanId = "SpaceMonkeySquadron",
                },
                VoiceConfiguration = new VoiceRtcConfiguration()
                {
                    AppId = "sunshine-rain-drop-very-beautifulday"
                }
            });
        }

        [When(@"I send a request to list the phone numbers")]
        public async Task WhenISendARequestToListThePhoneNumbers()
        {
            _listNumbers = await _sinchNumbers.List(new ListActiveNumbersRequest()
            {
                RegionCode = "US",
                Type = Types.Local
            });
        }

        [Then(@"the response contains ""(.*)"" phone numbers")]
        public void ThenTheResponseContainsPhoneNumbers(int count)
        {
            _listNumbers.ActiveNumbers.Should().HaveCount(count);
        }

        [When(@"I send a request to list all the phone numbers")]
        public void WhenISendARequestToListAllThePhoneNumbers()
        {
            _listAllNumbers = _sinchNumbers.ListAuto(new ListActiveNumbersRequest()
            {
                RegionCode = "US",
                Type = Types.Local
            });
        }

        [Then(@"the phone numbers list contains ""(.*)"" phone numbers")]
        public async Task ThenThePhoneNumbersListContainsPhoneNumbers(int count)
        {
            var counter = 0;
            await foreach (var _ in _listAllNumbers)
            {
                counter++;
            }

            counter.Should().Be(count);
        }

        [When(@"I send a request to retrieve the phone number ""(.*)""")]
        public void WhenISendARequestToRetrieveThePhoneNumber(string number)
        {
            _activeNumberOp = () => _sinchNumbers.Get(number);
        }

        [Then(@"the response contains details about the phone number ""(.*)""")]
        public async Task ThenTheResponseContainsDetailsAboutThePhoneNumber(string p0)
        {
            var number = await _activeNumberOp();
            number.PhoneNumber.Should().Be(p0);
            number.NextChargeDate.Should().Be(Helpers.ParseUtc("2024-06-06T14:42:42.677575Z"));
            number.ExpireAt.Should().BeNull();
            number.SmsConfiguration!.ServicePlanId.Should().Be("SpaceMonkeySquadron");
        }

        [Then(@"the response contains details about the phone number ""(.*)"" with an SMS provisioning error")]
        public async Task ThenTheResponseContainsDetailsAboutThePhoneNumberWithAnSmsProvisioningError(string p0)
        {
            var number = await _activeNumberOp();
            number.PhoneNumber.Should().Be(p0);
            number.NextChargeDate.Should().Be(Helpers.ParseUtc("2024-07-06T14:42:42.677575Z"));
            number.ExpireAt.Should().BeNull();
            number.SmsConfiguration!.ServicePlanId.Should().BeEmpty();
            number.SmsConfiguration!.ScheduledProvisioning!.Status.Should().Be(ProvisioningStatus.Failed);
            number.SmsConfiguration!.ScheduledProvisioning.ErrorCodes.Should()
                .BeEquivalentTo(new List<string>() { "SMS_PROVISIONING_FAILED" });
        }

        [Then(@"the response contains an error about the number ""(.*)"" not being a rented number")]
        public async Task ThenTheResponseContainsAnErrorAboutTheNumberNotBeingARentedNumber(string number)
        {
            var ex = await _activeNumberOp.Should().ThrowAsync<SinchApiException>();
            ex.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
            ex.Which.Details.First()!["resourceName"]!.AsValue().ToString().Should().Be(number);
        }

        [When(@"I send a request to release the phone number ""(.*)""")]
        public async Task WhenISendARequestToReleaseThePhoneNumber(string number)
        {
            _releaseResponse = await _sinchNumbers.Release(number);
        }

        [Then(@"the response contains details about the phone number ""(.*)"" to be released")]
        public void ThenTheResponseContainsDetailsAboutThePhoneNumberToBeReleased(string number)
        {
            _releaseResponse.PhoneNumber.Should().Be(number);
            _releaseResponse.ExpireAt.Should().Be(Helpers.ParseUtc("2024-06-06T14:42:42.677575Z"));
            _releaseResponse.NextChargeDate.Should().Be(Helpers.ParseUtc("2024-06-06T14:42:42.677575Z"));
        }
    }
}
