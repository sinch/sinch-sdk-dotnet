using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Numbers.Callbacks;

namespace Sinch.Tests.Features.Numbers
{
    [Binding]
    public class CallbackConfigurations
    {
        private ISinchNumbersCallbacks _sinchNumbersCallbacks;
        private CallbackConfiguration _callbackConfig;
        private Func<Task<CallbackConfiguration>> _callbackConfigOp;

        [Given(@"the Numbers service ""Callback Configuration"" is available")]
        public void GivenTheNumbersServiceIsAvailable()
        {
            _sinchNumbersCallbacks = Utils.SinchNumbersClient().Callbacks;
        }


        [When(@"I send a request to retrieve the callback configuration")]
        public async Task WhenISendARequestToRetrieveTheCallbackConfiguration()
        {
            _callbackConfig = await _sinchNumbersCallbacks.Get();
        }

        [Then(@"the response contains the project's callback configuration")]
        public void ThenTheResponseContainsTheProjectsCallbackConfiguration()
        {
            _callbackConfig.Should().BeEquivalentTo(new CallbackConfiguration()
            {
                HmacSecret = "0default-pass-word-*max-36characters",
                ProjectId = "12c0ffee-dada-beef-cafe-baadc0de5678"
            });
        }

        [When(@"I send a request to update the callback configuration with the secret ""(.*)""")]
        public void WhenISendARequestToUpdateTheCallbackConfigurationWithTheSecret(string hmacSecret)
        {
            _callbackConfigOp = () => _sinchNumbersCallbacks.Update(hmacSecret);
        }

        [Then(@"the response contains the updated project's callback configuration")]
        public async Task ThenTheResponseContainsTheUpdatedProjectsCallbackConfiguration()
        {
            var callbackConfig = await _callbackConfigOp();
            callbackConfig.Should().BeEquivalentTo(new CallbackConfiguration()
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
