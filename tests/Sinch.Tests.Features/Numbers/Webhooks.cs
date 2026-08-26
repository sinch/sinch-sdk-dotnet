using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Numbers;
using Sinch.Numbers.Hooks;

namespace Sinch.Tests.Features.Numbers
{
    [Binding]
    public class Webhooks
    {
        private ISinchNumbers _sinchNumbers;
        private readonly HttpClient _httpClient = new HttpClient();
        private HttpResponseMessage _eventResponse;
        private string _rawData;
        private const string SinchNumbersCallbackSecret = "strongPa$$PhraseWith36CharactersMax";

        [Given(@"the Numbers Webhooks handler is available")]
        public void GivenTheNumbersWebhooksHandlerIsAvailable()
        {
            _sinchNumbers = Utils.SinchNumbersClient();
        }

        [When(@"I send a request to trigger the ""success"" for ""PROVISIONING_TO_VOICE_PLATFORM"" event")]
        public async Task WhenISendARequestToTriggerTheForEvent()
        {
            _eventResponse = await _httpClient.GetAsync(
                Helpers.MOCKSERVER_NUMBERS_URL + "webhooks/numbers/provisioning_to_voice_platform/succeeded");
        }

        [Then(@"the header of the ""(.*)"" for ""(.*)"" event contains a valid signature")]
        public async Task ThenTheHeaderOfTheForEventContainsAValidSignature(string success, string eventType)
        {
            _rawData = await _eventResponse.Content.ReadAsStringAsync();

            // TODO: NUMBER_ORDER_PROCESSING 
            if (eventType == "NUMBER_ORDER_PROCESSING")
            {
                return;
            }
            _sinchNumbers.ValidateAuthenticationHeader(SinchNumbersCallbackSecret, _rawData, _eventResponse.Headers)
                .Should()
                .BeTrue();
        }

        [Then(@"the event describes a ""success"" for ""PROVISIONING_TO_VOICE_PLATFORM"" event")]
        public void ThenTheEventDescribesAForEvent()
        {
            var parsedEvent = JsonSerializer.Deserialize<Event>(_rawData);
            parsedEvent.EventType.Should().Be(EventType.ProvisioningToVoicePlatform);
            parsedEvent.Status.Should().Be(EventStatus.Succeeded);
            parsedEvent.FailureCode.Should().BeNull();
        }

        [When(@"I send a request to trigger the ""failure"" for ""PROVISIONING_TO_VOICE_PLATFORM"" event")]
        public async Task WhenISendARequestToTriggerTheFailureForEvent()
        {
            _eventResponse = await _httpClient.GetAsync(
                Helpers.MOCKSERVER_NUMBERS_URL + "webhooks/numbers/provisioning_to_voice_platform/failed");
        }

        [Then(@"the event describes a ""failure"" for ""PROVISIONING_TO_VOICE_PLATFORM"" event")]
        public void ThenTheEventDescribesAFailureForEvent()
        {
            var parsedEvent = JsonSerializer.Deserialize<Event>(_rawData);
            parsedEvent.EventType.Should().Be(EventType.ProvisioningToVoicePlatform);
            parsedEvent.Status.Should().Be(EventStatus.Failed);
            parsedEvent.FailureCode.Should().Be(FailureCode.ProvisioningToVoicePlatformFailed);
        }

        [When(@"I send a request to trigger the ""completed"" for ""NUMBER_ORDER_PROCESSING"" event")]
        public async Task WhenISendARequestToTriggerCompletedForEvent()
        {
            _eventResponse = await _httpClient.GetAsync(
                Helpers.MOCKSERVER_NUMBERS_URL + "webhooks/numbers/number_order_processing/completed");
        }

        [Then(@"the event describes a ""completed"" for ""NUMBER_ORDER_PROCESSING"" event")]
        public void ThenTheEventDescribesCompletedForEvent()
        {
            // TODO: Support NUMBER_ORDER_PROCESSING event
        }
    }
}
