using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Voice.Applications;
using Sinch.Voice.Applications.GetNumbers;
using Sinch.Voice.Applications.UnassignNumbers;
using Sinch.Voice.Applications.UpdateEventDestinations;
using Sinch.Voice.Applications.UpdateNumbers;
using NumberItem = Sinch.Voice.Applications.GetNumbers.NumberItem;

namespace Sinch.Tests.Features.Voice
{
    [Binding]
    public class Applications
    {
        private ISinchVoiceApplications _sinchVoiceApplications;
        private GetNumbersResponse _listNumbersResponse;
        private Func<Task> _assignNumberOp;
        private Func<Task> _unassignOp;
        private EventDestinations _eventDestinationsResponse;
        private Func<Task> _updateEventDestinationsOp;

        [When(@"I send a request to get information about my owned numbers")]
        public async Task WhenISendARequestToGetInformationAboutMyOwnedNumbers()
        {
            _listNumbersResponse = await _sinchVoiceApplications.GetNumbers();
        }

        [Then(@"the response contains details about the numbers that I own")]
        public void ThenTheResponseContainsDetailsAboutTheNumbersThatIOwn()
        {
            _listNumbersResponse.Should().BeEquivalentTo(new GetNumbersResponse()
            {
                Numbers = new List<NumberItem>()
                {
                    new NumberItem()
                    {
                        Number = "+12012222222",
                        Capability = Capability.Voice,
                    },
                    new NumberItem()
                    {
                        Number = "+12013333333",
                        Capability = Capability.Voice,
                        ApplicationKey = "ba5eba11-1dea-1337-babe-5a1ad00d1eaf"
                    },
                    new NumberItem()
                    {
                        Number = "+12014444444",
                        Capability = Capability.Voice,
                    },
                    new NumberItem()
                    {
                        Number = "+12015555555",
                        Capability = Capability.Voice,
                        ApplicationKey = "f00dcafe-abba-c0de-1dea-dabb1ed4caf3"
                    },
                }
            });
        }

        [Given(@"the Voice service ""Applications"" is available")]
        public void GivenTheVoiceServiceIsAvailable()
        {
            _sinchVoiceApplications = Utils.TestSinchVoiceClient.Applications;
        }

        [When(@"I send a request to assign some numbers to a Voice Application")]
        public void WhenISendARequestToAssignSomeNumbersToAVoiceApplication()
        {
            _assignNumberOp = () => _sinchVoiceApplications.AssignNumbers(new AssignNumbersRequest()
            {
                ApplicationKey = "f00dcafe-abba-c0de-1dea-dabb1ed4caf3",
                Numbers = new List<string>()
                {
                    "+12012222222"
                },
                Capability = Capability.Voice
            });
        }

        [Then(@"the assign numbers response contains no data")]
        public async Task ThenTheAssignNumbersResponseContainsNoData()
        {
            await _assignNumberOp.Should().NotThrowAsync();
        }

        [When(@"I send a request to unassign a number from a Voice Application")]
        public void WhenISendARequestToUnassignANumberFromAVoiceApplication()
        {
            _unassignOp = () => _sinchVoiceApplications.UnassignNumber(new UnassignNumberRequest()
            {
                Number = "+12012222222"
            });
        }

        [Then(@"the unassign number response contains no data")]
        public async Task ThenTheUnassignNumberResponseContainsNoData()
        {
            await _unassignOp.Should().NotThrowAsync();
        }

        [When(@"I send a request to get the callback URLs associated to an application")]
        public async Task WhenISendARequestToGetTheCallbackUrLsAssociatedToAnApplication()
        {
            _eventDestinationsResponse = await _sinchVoiceApplications.GetEventDestinations("f00dcafe-abba-c0de-1dea-dabb1ed4caf3");
        }

        [Then(@"the response contains callback URLs details")]
        public void ThenTheResponseContainsCallbackUrLsDetails()
        {
            _eventDestinationsResponse.Should().BeEquivalentTo(new EventDestinations()
            {
                Url = new EventDestinationTarget()
                {
                    Primary = "https://my.callback-server.com/voice",
                    Fallback = "https://my.fallback-server.com/voice"
                }
            });
        }

        [When(@"I send a request to update the callback URLs associated to an application")]
        public void WhenISendARequestToUpdateTheCallbackUrLsAssociatedToAnApplication()
        {
            _updateEventDestinationsOp = () => _sinchVoiceApplications.UpdateEventDestinations(new UpdateEventDestinationsRequest()
            {
                ApplicationKey = "f00dcafe-abba-c0de-1dea-dabb1ed4caf3",
                Url = new EventDestinationTarget()
                {
                    Primary = "https://my-new.callback-server.com/voice"
                }
            });
        }

        [Then(@"the update callback URLs response contains no data")]
        public async Task ThenTheUpdateCallbackUrLsResponseContainsNoData()
        {
            await _updateEventDestinationsOp.Should().NotThrowAsync();
        }
    }
}
