using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Numbers.EventDestinations;

namespace Sinch.Tests.Features.Numbers
{
    [Binding]
    public class CallbackConfigurations
    {
        private ISinchNumbersEventDestinations _sinchNumbersEventDestinations;
        private EventDestination _eventDestination;
        private Func<Task<EventDestination>> _eventDestinationOp;

        [Given(@"the Numbers service ""Callback Configuration"" is available")]
        public void GivenTheNumbersServiceIsAvailable()
        {
            _sinchNumbersEventDestinations = Utils.SinchNumbersClient().EventDestinations;
        }


        [When(@"I send a request to retrieve the callback configuration")]
        public async Task WhenISendARequestToRetrieveTheCallbackConfiguration()
        {
            _eventDestination = await _sinchNumbersEventDestinations.Get();
        }

        [Then(@"the response contains the project's callback configuration")]
        public void ThenTheResponseContainsTheProjectsCallbackConfiguration()
        {
            _eventDestination.Should().BeEquivalentTo(new EventDestination()
            {
                HmacSecret = "0default-pass-word-*max-36characters",
                ProjectId = "12c0ffee-dada-beef-cafe-baadc0de5678"
            });
        }

        [When(@"I send a request to update the callback configuration with the secret ""(.*)""")]
        public void WhenISendARequestToUpdateTheCallbackConfigurationWithTheSecret(string hmacSecret)
        {
            _eventDestinationOp = () => _sinchNumbersEventDestinations.Update(hmacSecret);
        }

        [Then(@"the response contains the updated project's callback configuration")]
        public async Task ThenTheResponseContainsTheUpdatedProjectsCallbackConfiguration()
        {
            var eventDestination = await _eventDestinationOp();
            eventDestination.Should().BeEquivalentTo(new EventDestination()
            {
                ProjectId = "12c0ffee-dada-beef-cafe-baadc0de5678",
                HmacSecret = "strongPa$$PhraseWith36CharactersMax"
            });
        }

        [Then(@"the response contains an error")]
        public void ThenTheResponseContainsAnError()
        {
            _eventDestinationOp.Should().ThrowAsync<SinchApiException>()
                .Where(x => x.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
