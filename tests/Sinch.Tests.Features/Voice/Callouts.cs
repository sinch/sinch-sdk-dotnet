using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Voice;
using Sinch.Voice.Callouts;
using Sinch.Voice.Callouts.Callout;

namespace Sinch.Tests.Features.Voice
{
    [Binding]
    public class Callouts
    {
        private ISinchVoiceCallout _sinchVoiceCallouts;
        private CalloutResponse _ttsCalloutResponse;
        private CalloutResponse _conferenceCalloutResponse;
        private CalloutResponse _customCalloutResponse;

        [Given(@"the Voice service ""Callouts"" is available")]
        public void GivenTheVoiceServiceIsAvailable()
        {
            _sinchVoiceCallouts = Utils.TestSinchVoiceClient.Callouts;
        }

        [When(@"I send a request to make a TTS call")]
        public async Task WhenISendARequestToMakeAttsCall()
        {
            _ttsCalloutResponse = await _sinchVoiceCallouts.Tts(new TextToSpeechCalloutRequest()
            {
                Cli = "+12015555555",
                Destination = new Destination()
                {
                    Type = DestinationType.Number,
                    Endpoint = "+12017777777"
                },
                Locale = "en-US",
                Text = "Hello, this is a call from Sinch."
            });
        }

        [Then(@"the callout response contains the TTS call ID")]
        public void ThenTheCalloutResponseContainsTheTtsCallId()
        {
            _ttsCalloutResponse.Should().BeEquivalentTo(new CalloutResponse()
            {
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000001"
            });
        }

        [When(@"I send a request to make a Conference call with the ""Callout"" service")]
        public async Task WhenISendARequestToMakeAConferenceCallWithTheService()
        {
            _conferenceCalloutResponse = await _sinchVoiceCallouts.Conference(new ConferenceCalloutRequest()
            {
                Cli = "+12015555555",
                Destination = new Destination()
                {
                    Endpoint = "+12017777777",
                    Type = DestinationType.Number,
                },
                Locale = "en-US",
                ConferenceId = "myConferenceId-E2E",
                Greeting = "Welcome to this conference call.",
                MohClass = MohClass.Music1,
            });
        }

        [Then(@"the callout response contains the Conference call ID")]
        public void ThenTheCalloutResponseContainsTheConferenceCallId()
        {
            _conferenceCalloutResponse.Should().BeEquivalentTo(new CalloutResponse()
            {
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000002"
            });
        }

        [When(@"I send a request to make a Custom call")]
        public async Task WhenISendARequestToMakeACustomCall()
        {
            // TODO: implement SVAML typed classes in https://tickets.sinch.com/browse/DEVEXP-281
            _customCalloutResponse = await _sinchVoiceCallouts.Custom(new CustomCalloutRequest()
            {
                Cli = "+12015555555",
                Destination = new Destination()
                {
                    Type = DestinationType.Number,
                    Endpoint = "+12017777777"
                },
                Custom = "Custom text",
                Ice = JsonNode.Parse(
                        "{\"action\":{\"name\":\"connectPstn\",\"number\":\"+12017777777\",\"cli\":\"+12015555555\"},\"instructions\":[{\"name\":\"say\",\"text\":\"Welcome to Sinch.\",\"locale\":\"en-US/male\"},{\"name\":\"startRecording\",\"options\":{\"destinationUrl\":\"To specify\",\"credentials\":\"To specify\"}}]}")
                    !.AsObject()!,
                Ace = JsonNode.Parse(
                        "{\"action\":{\"name\":\"runMenu\",\"locale\":\"Kimberly\",\"enableVoice\":true,\"barge\":true,\"menus\":[{\"id\":\"main\",\"mainPrompt\":\"#tts[Welcome to the main menu. Press 1 to confirm order or 2 to cancel]\",\"repeatPrompt\":\"#tts[We didn't get your input, please try again]\",\"timeoutMills\":5000,\"options\":[{\"dtmf\":\"1\",\"action\":\"menu(confirm)\"},{\"dtmf\":\"2\",\"action\":\"return(cancel)\"}]},{\"id\":\"confirm\",\"mainPrompt\":\"#tts[Thank you for confirming your order. Enter your 4-digit PIN.]\",\"maxDigits\":4}]}}")
                    !.AsObject()!,
                Pie = "https://callback-server.com/voice"
            });
        }

        [Then(@"the callout response contains the Custom call ID")]
        public void ThenTheCalloutResponseContainsTheCustomCallId()
        {
            _customCalloutResponse.Should().BeEquivalentTo(new CalloutResponse()
            {
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000003"
            });
        }
    }
}
