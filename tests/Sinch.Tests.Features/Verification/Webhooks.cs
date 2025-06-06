using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Verification;
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
        private ISinchVerificationClient _sinchVerificationClient;
        private string _rawBody;

        [Given(@"the Verification Webhooks handler is available")]
        public void GivenTheVerificationWebhooksHandlerIsAvailable()
        {
            _sinchVerificationClient = Utils.SinchVerificationClient;
        }

        [When(@"I send a request to trigger a ""Verification Request"" event")]
        public async Task WhenISendARequestToTriggerAEvent()
        {
            _verificationRequestResponseMessage =
                await _httpClient.GetAsync("http://localhost:3018/webhooks/verification/verification-request-event");
        }

        [Then(@"the header of the Verification event ""Verification Request"" contains a valid authorization")]
        public async Task ThenTheHeaderOfTheVerificationEventContainsAValidAuthorization()
        {
            _rawBody = await _verificationRequestResponseMessage.Content.ReadAsStringAsync();
            _sinchVerificationClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/verification",
                _verificationRequestResponseMessage.Headers, _verificationRequestResponseMessage.Content.Headers,
                _rawBody).Should().BeTrue();
        }

        [Then(@"the Verification event describes a ""Verification Request"" event type")]
        public void ThenTheVerificationEventDescribesAEventType()
        {
            // TODO: schema of oas and api response diverge: https://tickets.sinch.com/browse/DEVEXP-946
            var verificationEvent = JsonSerializer.Deserialize<VerificationRequestEvent>(_rawBody);
            verificationEvent.As<VerificationRequestEvent>().Should().BeEquivalentTo(new VerificationRequestEvent
            {
                Id = "1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                Event = "VerificationRequestEvent",
                Method = VerificationMethod.Sms,
                Identity = Identity.Number("+33612345678"),
                Price = new PriceDetail()
                {
                    Amount = 0.0453d,
                    CurrencyId = "EUR"
                }
            });
        }

        [When(@"I send a request to trigger a ""Verification Result"" event")]
        public async Task WhenISendARequestToTriggerAVerificationResultEvent()
        {
            _verificationResultResponse =
                await _httpClient.GetAsync("http://localhost:3018/webhooks/verification/verification-result-event");
        }

        [Then(@"the header of the Verification event ""Verification Result"" contains a valid authorization")]
        public async Task ThenTheHeaderOfTheVerificationResultEventContainsAValidAuthorization()
        {
            _rawBody = await _verificationResultResponse.Content.ReadAsStringAsync();
            _sinchVerificationClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/verification",
                _verificationResultResponse.Headers, _verificationResultResponse.Content.Headers,
                _rawBody).Should().BeTrue();
        }

        [Then(@"the Verification event describes a ""Verification Result"" event type")]
        public void ThenTheVerificationEventDescribesAResultEventType()
        {

            var resultEvent = JsonSerializer.Deserialize<VerificationResultEvent>(_rawBody);
            // TODO: schema of oas and api response diverge: https://tickets.sinch.com/browse/DEVEXP-946
            resultEvent.Should().BeEquivalentTo(new VerificationResultEvent()
            {
                Id = "1ce0ffee-c0de-5eed-d00d-f00dfeed1337",
                Event = "VerificationResultEvent",
                Method = VerificationMethodEx.Sms,
                Identity = Identity.Number("+33612345678"),
                Status = VerificationStatus.Successful
            });
        }
    }
}
