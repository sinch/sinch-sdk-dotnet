using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Numbers.EventDestination;

namespace Sinch.Tests.Features.Numbers
{
    [Binding]
    public class CallbackConfigurations
    {
        private ISinchNumbersEventDestination _sinchNumbersEventDestination;
        private EventDestination _callbackConfig;
        private Func<Task<EventDestination>> _callbackConfigOp;

        [Given(@"the Numbers service ""Callback Configuration"" is available")]
        public void GivenTheNumbersServiceIsAvailable()
        {
            _sinchNumbersEventDestination = Utils.SinchNumbersClient().EventDestination;
        }


        [When(@"I send a request to retrieve the callback configuration")]
        public async Task WhenISendARequestToRetrieveTheCallbackConfiguration()
        {
            _callbackConfig = await _sinchNumbersEventDestination.Get();
        }

        [Then(@"the response contains the project's callback configuration")]
        public void ThenTheResponseContainsTheProjectsCallbackConfiguration()
        {
            _callbackConfig.Should().BeEquivalentTo(new EventDestination()
            {
                HmacSecret = "0default-pass-word-*max-36characters",
                ProjectId = "12c0ffee-dada-beef-cafe-baadc0de5678"
            });
        }

        [When(@"I send a request to update the callback configuration with the secret ""(.*)""")]
        public void WhenISendARequestToUpdateTheCallbackConfigurationWithTheSecret(string hmacSecret)
        {
            _callbackConfigOp = () => _sinchNumbersEventDestination.Update(hmacSecret);
        }

        [Then(@"the response contains the updated project's callback configuration")]
        public async Task ThenTheResponseContainsTheUpdatedProjectsCallbackConfiguration()
        {
            var callbackConfig = await _callbackConfigOp();
            callbackConfig.Should().BeEquivalentTo(new EventDestination()
            {
                ProjectId = "12c0ffee-dada-beef-cafe-baadc0de5678",
                HmacSecret = "strongPa$$PhraseWith36CharactersMax"
            });
        }

        [Then(@"the response contains an error")]
        public void ThenTheResponseContainsAnError()
        {
            _callbackConfigOp.Should().ThrowAsync<SinchApiException>()
                .Where(x => x.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
