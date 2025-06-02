using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Verification.Common;
using Sinch.Verification.Hooks;

namespace Sinch.Tests.Features.Verification
{
    [Binding]
    public class Webhooks
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private HttpResponseMessage _verificationRequestResponseMessage;
        private HttpResponseMessage _verificationResultResponse;

        [Given(@"the Verification Webhooks handler is available")]
        public void GivenTheVerificationWebhooksHandlerIsAvailable()
        {
        }

        [When(@"I send a request to trigger a ""Verification Request"" event")]
        public async Task WhenISendARequestToTriggerAEvent()
        {
            _verificationRequestResponseMessage =
                await _httpClient.GetAsync("http://localhost:3018/webhooks/verification/verification-request-event");
        }

        [Then(@"the header of the Verification event ""Verification Request"" contains a valid authorization")]
        public void ThenTheHeaderOfTheVerificationEventContainsAValidAuthorization()
        {
            // TODO: implement header verification https://tickets.sinch.com/browse/DEVEXP-944
        }

        [Then(@"the Verification event describes a ""Verification Request"" event type")]
        public async Task ThenTheVerificationEventDescribesAEventType()
        {
            // TODO: fix schema
            var raw = await _verificationRequestResponseMessage.Content.ReadAsStringAsync();
            var verificationEvent = JsonSerializer.Deserialize<VerificationRequestEvent>(raw, new JsonSerializerOptions()
            {
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
            });
            verificationEvent.As<VerificationRequestEvent>().Should().BeEquivalentTo(new VerificationRequestEvent
            {
                Id = "1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                Event = "VerificationRequestEvent",
                Method = VerificationMethod.Sms,
                Identity = Identity.Number("+33612345678"),
                Price = new PriceDetail()
                {
                    Amount = 0.453d,
                    CurrencyId = "EUR"
                },
                Reference = null,
                Custom = null,
                AcceptLanguage = null
            });
        }

        [When(@"I send a request to trigger a ""Verification Result"" event")]
        public async Task WhenISendARequestToTriggerAVerificationResultEvent()
        {
            _verificationResultResponse =
                await _httpClient.GetAsync("http://localhost:3018/webhooks/verification/verification-result-event");
        }

        [Then(@"the header of the Verification event ""(.*)"" contains a valid authorization")]
        public void ThenTheHeaderOfTheVerificationEventContainsAValidAuthorization(string p0)
        {
            // TODO: implement hooks validation https://tickets.sinch.com/browse/DEVEXP-944
        }

        [Then(@"the Verification event describes a ""Verification Result"" event type")]
        public async Task ThenTheVerificationEventDescribesAResultEventType()
        {
            var raw = await _verificationResultResponse.Content.ReadAsStringAsync();
            var resultEvent = JsonSerializer.Deserialize<VerificationResultEvent>(raw, new JsonSerializerOptions()
            {
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
            });
            // TODO: update schema with Verified in identity?
            resultEvent.Should().BeEquivalentTo(new VerificationRequestEvent
            {
                Id = null,
                Event = null,
                Method = null,
                Identity = null,
                Price = null,
                Reference = null,
                Custom = null,
                AcceptLanguage = null
            });
        }
    }
}
