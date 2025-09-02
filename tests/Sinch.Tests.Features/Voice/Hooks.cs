using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Voice;
using Sinch.Voice.Callouts.Callout;
using Sinch.Voice.Calls;
using Sinch.Voice.Hooks;
using DestinationType = Sinch.Voice.Hooks.DestinationType;

namespace Sinch.Tests.Features.Voice
{
    [Binding]
    public class Hooks
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private ISinchVoiceClient _voiceClient;
        private HttpResponseMessage _pieReturnResponse;
        private string _rawPieSequenceContent;
        private HttpResponseMessage _pieSequenceResponse;
        private HttpResponseMessage _diceResponse;
        private string _rawDiceContent;
        private HttpResponseMessage _aceResponse;
        private string _rawAceContent;
        private HttpResponseMessage _iceResponse;
        private string _rawIceContent;
        private HttpResponseMessage _eventRecordingFinishedResponse;
        private string _rawEventRecordAvailableContent;
        private HttpResponseMessage _eventRecordingAvailableResponse;
        private HttpResponseMessage _eventTranscriptionAvailableResponse;
        private string _rawEventTransactionContent;

        [Given(@"the Voice Webhooks handler is available")]
        public void GivenTheVoiceWebhooksHandlerIsAvailable()
        {
            _voiceClient = new SinchClient(
                new SinchClientConfiguration
                {
                    SinchOptions = new SinchOptions
                    {
                        ApiUrlOverrides = new ApiUrlOverrides()
                        {
                            VoiceUrl = "http://localhost:3019",
                            VoiceApplicationManagementUrl = "http://localhost:3020"
                        }
                    },
                    VoiceConfiguration = new SinchVoiceConfiguration()
                    {
                        AppKey = "appKey",
                        AppSecret = "YXBwU2VjcmV0"
                    }
                }
            ).Voice;
        }

        [When(@"I send a request to trigger a ""PIE"" event with a ""return"" type")]
        public async Task WhenISendARequestToTriggerAEventWithAType()
        {
            _pieReturnResponse = await _httpClient.GetAsync("http://localhost:3019/webhooks/voice/pie-return");
        }

        [Then(@"the header of the ""PIE"" event with a ""return"" type contains a valid authorization")]
        public async Task ThenTheHeaderOfTheEventWithATypeContainsAValidAuthorization()
        {
            _rawPieSequenceContent = await _pieReturnResponse.Content.ReadAsStringAsync();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/voice", _pieReturnResponse.Headers,
                _pieReturnResponse.Content.Headers,
                _rawPieSequenceContent).Should().BeTrue();
        }

        [Then(@"the Voice event describes a ""PIE"" event with a ""return"" type")]
        public void ThenTheVoiceEventDescribesAEventWithAType()
        {
            _voiceClient.ParseEvent(_rawPieSequenceContent).As<PromptInputEvent>().Should().BeEquivalentTo(
                new PromptInputEvent
                {
                    CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000013",
                    Timestamp = Helpers.ParseUtc("2024-06-06T17:35:01Z"),
                    MenuResult = new MenuResult()
                    {
                        InputMethod = InputMethod.Dtmf,
                        Value = "cancel",
                        MenuId = "main",
                        Type = MenuType.Return,
                    },
                    Version = 1,
                    ApplicationKey = "f00dcafe-abba-c0de-1dea-dabb1ed4caf3",
                    Custom = "Custom text"
                });
        }

        [When(@"I send a request to trigger a ""PIE"" event with a ""sequence"" type")]
        public async Task WhenISendARequestToTriggerAPieEventWithATypeSequence()
        {
            _pieSequenceResponse = await _httpClient.GetAsync("http://localhost:3019/webhooks/voice/pie-sequence");
        }

        [Then(@"the header of the ""PIE"" event with a ""sequence"" type contains a valid authorization")]
        public async Task ThenTheHeaderOfTheEventWithAPieTypeSequenceContainsAValidAuthorization()
        {
            _rawPieSequenceContent = await _pieSequenceResponse.Content.ReadAsStringAsync();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/voice", _pieSequenceResponse.Headers,
                _pieSequenceResponse.Content.Headers,
                _rawPieSequenceContent).Should().BeTrue();
        }

        [Then(@"the Voice event describes a ""PIE"" event with a ""sequence"" type")]
        public void ThenTheVoiceEventDescribesAPieEventWithASequenceType()
        {
            _voiceClient.ParseEvent(_rawPieSequenceContent).As<PromptInputEvent>().Should().BeEquivalentTo(
                new PromptInputEvent
                {
                    CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000023",
                    Timestamp = Helpers.ParseUtc("2024-06-06T17:35:58Z"),
                    MenuResult = new MenuResult()
                    {
                        Type = MenuType.Sequence,
                        Value = "1234",
                        MenuId = "confirm",
                        InputMethod = InputMethod.Dtmf
                    },
                    Version = 1,
                    ApplicationKey = "f00dcafe-abba-c0de-1dea-dabb1ed4caf3",
                    Custom = "Custom text"
                });
        }

        [When(@"I send a request to trigger a ""DICE"" event")]
        public async Task WhenISendARequestToTriggerAEvent()
        {
            _diceResponse = await _httpClient.GetAsync("http://localhost:3019/webhooks/voice/dice");
        }

        [Then(@"the header of the ""DICE"" event contains a valid authorization")]
        public async Task ThenTheHeaderOfTheEventDiceContainsAValidAuthorization()
        {
            _rawDiceContent = await _diceResponse.Content.ReadAsStringAsync();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/voice", _diceResponse.Headers,
                _diceResponse.Content.Headers,
                _rawDiceContent).Should().BeTrue();
        }

        [Then(@"the Voice event describes a ""DICE"" event")]
        public void ThenTheVoiceEventDescribesAEvent()
        {
            _voiceClient.ParseEvent(_rawDiceContent).Should().BeEquivalentTo(new DisconnectedCallEvent
            {
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000033",
                Timestamp = Helpers.ParseUtc("2024-06-06T16:59:42Z"),
                Reason = CallResultReason.ManagerHangUp,
                Result = CallResult.Answered,
                Version = 1,
                Custom = "Custom text",
                Debit = new Rate()
                {
                    Amount = 0.0095m,
                    CurrencyId = "EUR"
                },
                UserRate = new Rate()
                {
                    Amount = 0.0095m,
                    CurrencyId = "EUR"
                },
                To = new To()
                {
                    Endpoint = "12017777777",
                    Type = DestinationType.Number
                },
                Duration = 12,
                From = "12015555555",
                ApplicationKey = "f00dcafe-abba-c0de-1dea-dabb1ed4caf3"
            });
        }

        [When(@"I send a request to trigger a ""ACE"" event")]
        public async Task WhenISendARequestToTriggerAceEvent()
        {
            _aceResponse = await _httpClient.GetAsync("http://localhost:3019/webhooks/voice/ace");
        }

        [Then(@"the header of the ""ACE"" event contains a valid authorization")]
        public async Task ThenTheHeaderOfTheEventContainsAValidAuthorization()
        {
            _rawAceContent = await _aceResponse.Content.ReadAsStringAsync();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/voice", _aceResponse.Headers,
                _aceResponse.Content.Headers,
                _rawAceContent).Should().BeTrue();
        }

        [Then(@"the Voice event describes a ""ACE"" event")]
        public void ThenTheVoiceEventDescribesAceEvent()
        {
            _voiceClient.ParseEvent(_rawAceContent).Should().BeEquivalentTo(new AnsweredCallEvent
            {
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000043",
                CallResourceUrl = null,
                Timestamp = Helpers.ParseUtc("2024-06-06T17:10:34Z"),
                Version = 1,
                Custom = "Custom text",
                ApplicationKey = "f00dcafe-abba-c0de-1dea-dabb1ed4caf3",
                Amd = null
            });
        }

        [When(@"I send a request to trigger a ""ICE"" event")]
        public async Task WhenISendARequestToTriggerIceEvent()
        {
            _iceResponse = await _httpClient.GetAsync("http://localhost:3019/webhooks/voice/ice");
        }

        [Then(@"the header of the ""ICE"" event contains a valid authorization")]
        public async Task ThenTheHeaderOfTheIceEventContainsAValidAuthorization()
        {
            _rawIceContent = await _iceResponse.Content.ReadAsStringAsync();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/voice", _iceResponse.Headers,
                _iceResponse.Content.Headers,
                _rawIceContent).Should().BeTrue();
        }

        [Then(@"the Voice event describes a ""ICE"" event")]
        public void ThenTheVoiceEventDescribesAIceEvent()
        {
            _voiceClient.ParseEvent(_rawIceContent).As<IncomingCallEvent>().Should().BeEquivalentTo(
                new IncomingCallEvent()
                {
                    CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000053",
                    CallResourceUrl =
                        "https://calling-use1.api.sinch.com/calling/v1/calls/id/1ce0ffee-ca11-ca11-ca11-abcdef000053",
                    Timestamp = Helpers.ParseUtc("2024-06-06T17:20:14Z"),
                    Version = 1,
                    UserRate = new Rate()
                    {
                        CurrencyId = "USD",
                        Amount = 0.0m,
                    },
                    Cli = "12015555555",
                    To = new To()
                    {
                        Type = DestinationType.Did,
                        Endpoint = "+12017777777"
                    },
                    Domain = Domain.Pstn,
                    ApplicationKey = "f00dcafe-abba-c0de-1dea-dabb1ed4caf3",
                    OriginationType = Domain.Pstn,
                    Rdnis = string.Empty
                });
        }

        [When(@"I send a request to trigger a ""recording_finished"" event")]
        public async Task WhenISendARequestToTriggerARecordFinishedEvent()
        {
            _eventRecordingFinishedResponse =
                await _httpClient.GetAsync("http://localhost:3019/webhooks/voice/notify/recording_finished");
        }

        [Then(@"the header of the ""recording_finished"" event contains a valid authorization")]
        public async Task ThenTheHeaderOfTheRecordingFinishedEventContainsAValidAuthorization()
        {
            _rawEventRecordAvailableContent = await _eventRecordingFinishedResponse.Content.ReadAsStringAsync();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/voice",
                _eventRecordingFinishedResponse.Headers,
                _eventRecordingFinishedResponse.Content.Headers,
                _rawEventRecordAvailableContent).Should().BeTrue();
        }


        [Then(@"the Voice event describes a ""notify"" event with a ""recording_finished"" type")]
        public void ThenTheVoiceEventDescribesANotifyEventWithARecordFinishedType()
        {
            _voiceClient.ParseEvent(_rawEventRecordAvailableContent).As<NotificationEvent>().Should().BeEquivalentTo(
                new NotificationEvent()
                {
                    CallId = "33dd8e62-0ac6-4e0c-a89f-36d121f861f9",
                    Version = 1,
                    Type = "recording_finished",
                });
        }

        [When(@"I send a request to trigger a ""recording_available"" event")]
        public async Task WhenISendARequestToTriggerARecordingAvailableEvent()
        {
            _eventRecordingAvailableResponse =
                await _httpClient.GetAsync("http://localhost:3019/webhooks/voice/notify/recording_available");
        }

        [Then(@"the header of the ""recording_available"" event contains a valid authorization")]
        public async Task ThenTheHeaderOfTheRecordingAvailableEventContainsAValidAuthorization()
        {
            _rawEventRecordAvailableContent = await _eventRecordingAvailableResponse.Content.ReadAsStringAsync();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/voice",
                _eventRecordingAvailableResponse.Headers,
                _eventRecordingAvailableResponse.Content.Headers,
                _rawEventRecordAvailableContent).Should().BeTrue();
        }

        [Then(@"the Voice event describes a ""notify"" event with a ""recording_available"" type")]
        public void ThenTheVoiceEventNotifyDescribesAEventWithARecordingAvailableType()
        {
            _voiceClient.ParseEvent(_rawEventRecordAvailableContent).As<NotificationEvent>().Should().BeEquivalentTo(
                new NotificationEvent()
                {
                    CallId = "33dd8e62-0ac6-4e0c-a89f-36d121f861f9",
                    Version = 1,
                    Type = "recording_available",
                    Destination = "azure://sinchsdk/voice-recordings/my-recording.mp3"
                });
        }

        [When(@"I send a request to trigger a ""transcription_available"" event")]
        public async Task WhenISendARequestToTriggerATranscriptionAvailableEvent()
        {
            _eventTranscriptionAvailableResponse =
                await _httpClient.GetAsync("http://localhost:3019/webhooks/voice/notify/transcription_available");
        }

        [Then(@"the header of the ""transcription_available"" event contains a valid authorization")]
        public async Task ThenTheHeaderOfTheTranscriptionAvailableEventContainsAValidAuthorization()
        {
            _rawEventTransactionContent = await _eventTranscriptionAvailableResponse.Content.ReadAsStringAsync();
            _voiceClient.ValidateAuthenticationHeader(HttpMethod.Post, "/webhooks/voice",
                _eventTranscriptionAvailableResponse.Headers,
                _eventTranscriptionAvailableResponse.Content.Headers,
                _rawEventTransactionContent).Should().BeTrue();
        }

        [Then(@"the Voice event describes a ""notify"" event with a ""transcription_available"" type")]
        public void ThenTheVoiceEventDescribesANotifyEventWithATranscriptionAvailableType()
        {
            _voiceClient.ParseEvent(_rawEventTransactionContent).As<NotificationEvent>().Should().BeEquivalentTo(
                new NotificationEvent()
                {
                    CallId = "33dd8e62-0ac6-4e0c-a89f-36d121f861f9",
                    Version = 1,
                    Type = "transcription_available",
                    Destination = "azure://sinchsdk/voice-recordings/my-recording-transcript.json"
                });
        }
    }
}
