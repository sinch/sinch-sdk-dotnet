using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Numbers;
using Sinch.Numbers.SinchEvents;

namespace Sinch.Tests.Features.Numbers
{
    [Binding]
    public class Webhooks
    {
        private INumbersSinchEvents _sinchEvents;
        private readonly HttpClient _httpClient = new();
        private HttpResponseMessage _eventResponse;
        private string _rawData;
        private const string SinchNumbersCallbackSecret = "strongPa$$PhraseWith36CharactersMax";

        [Given(@"the Numbers Webhooks handler is available")]
        public void GivenTheNumbersWebhooksHandlerIsAvailable()
        {
            _sinchEvents = Utils.SinchNumbersClient().SinchEvents;
        }

        [When(@"I send a request to trigger the ""success"" for ""PROVISIONING_TO_VOICE_PLATFORM"" event")]
        public async Task WhenISendARequestToTriggerTheForEvent()
        {
            _eventResponse = await _httpClient.GetAsync(
                "http://localhost:3013/webhooks/numbers/provisioning_to_voice_platform/succeeded");
        }

        [Then(@"the header of the ""(.*)"" for ""(.*)"" event contains a valid signature")]
        public async Task ThenTheHeaderOfTheForEventContainsAValidSignature(string success, string p1)
        {
            _rawData = await _eventResponse.Content.ReadAsStringAsync();
            var headers = _eventResponse.Headers
                .ToDictionary(h => h.Key, h => h.Value.AsEnumerable(),
                    System.StringComparer.OrdinalIgnoreCase);
            _sinchEvents.ValidateAuthenticationHeader(SinchNumbersCallbackSecret, headers, _rawData)
                .Should()
                .BeTrue();
        }

        [Then(@"the event describes a ""success"" for ""PROVISIONING_TO_VOICE_PLATFORM"" event")]
        public void ThenTheEventDescribesAForEvent()
        {
            var parsedEvent = _sinchEvents.ParseEvent(_rawData) as NumbersSinchEvent;
            parsedEvent.EventType.Should().Be(EventType.ProvisioningToVoicePlatform);
            parsedEvent.Status.Should().Be(EventStatus.Succeeded);
            parsedEvent.FailureCode.Should().BeNull();
        }

        [When(@"I send a request to trigger the ""failure"" for ""PROVISIONING_TO_VOICE_PLATFORM"" event")]
        public async Task WhenISendARequestToTriggerTheFailureForEvent()
        {
            _eventResponse = await _httpClient.GetAsync(
                "http://localhost:3013/webhooks/numbers/provisioning_to_voice_platform/failed");
        }

        [Then(@"the event describes a ""failure"" for ""PROVISIONING_TO_VOICE_PLATFORM"" event")]
        public void ThenTheEventDescribesAFailureForEvent()
        {
            var parsedEvent = _sinchEvents.ParseEvent(_rawData) as NumbersSinchEvent;
            parsedEvent.EventType.Should().Be(EventType.ProvisioningToVoicePlatform);
            parsedEvent.Status.Should().Be(EventStatus.Failed);
            parsedEvent.FailureCode.Should().Be(FailureCode.ProvisioningToVoicePlatformFailed);
        }
    }
}
