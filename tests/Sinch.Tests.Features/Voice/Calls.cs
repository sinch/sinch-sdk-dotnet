using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Voice;
using Sinch.Voice.Calls;
using Sinch.Voice.Calls.Actions;
using Sinch.Voice.Calls.Instructions;
using Sinch.Voice.Calls.Manage;
using Sinch.Voice.Calls.Update;
using Sinch.Voice.Common;

namespace Sinch.Tests.Features.Voice
{
    [Binding]
    public class Calls
    {
        private ISinchVoiceCalls _sinchVoiceCalls;
        private Call _callInformation;
        private Func<Task> _updateRequest;
        private Func<Task> _updateNonExistentOp;
        private Func<Task> _manageWithCallLeg;


        [Given(@"the Voice service ""Calls"" is available")]
        public void GivenTheVoiceServiceIsAvailable()
        {
            _sinchVoiceCalls = Utils.TestSinchVoiceClient.Calls;
        }

        [When(@"I send a request to get a call's information")]
        public async Task WhenISendARequestToGetACallsInformation()
        {
            _callInformation = await _sinchVoiceCalls.Get("1ce0ffee-ca11-ca11-ca11-abcdef000003");
        }

        [Then(@"the response contains the information about the call")]
        public void ThenTheResponseContainsTheInformationAboutTheCall()
        {
            _callInformation.Should().BeEquivalentTo(new Call
            {
                To = new Destination
                {
                    Type = ParticipantType.Number,
                    Endpoint = "+12017777777"
                },
                Domain = CallDomain.Pstn,
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000003",
                Duration = 14,
                Status = CallStatus.Final,
                Result = CallResult.Answered,
                Reason = CallResultReason.ManagerHangUp,
                Timestamp = DateTime.Parse("2024-06-06T17:36:00"),
                Custom = "Custom text",
                UserRate = new Price
                {
                    CurrencyId = "EUR",
                    Amount = 0.1758f
                },
                Debit = new Price
                {
                    CurrencyId = "EUR",
                    Amount = 0.1758f
                }
            });
        }

        [When(@"I send a request to update a call")]
        public void WhenISendARequestToUpdateACall()
        {
            _updateRequest = () => _sinchVoiceCalls.Update(new UpdateCallRequest()
            {
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000022",
                Action = new Hangup(),
                Instructions = new List<IInstruction>()
                {
                    new Say()
                    {
                        Text = "Sorry, the conference has been cancelled. The call will end now.",
                        Locale = "en-US",
                    }
                }
            });
        }

        [Then(@"the update call response contains no data")]
        public async Task ThenTheUpdateCallResponseContainsNoData()
        {
            await _updateRequest.Should().NotThrowAsync();
        }

        [When(@"I send a request to update a call that doesn't exist")]
        public void WhenISendARequestToUpdateACallThatDoesntExist()
        {
            _updateNonExistentOp = () => _sinchVoiceCalls.Update(new UpdateCallRequest()
            {
                CallId = "not-existing-callId",
                Action = new Hangup(),
                Instructions = new List<IInstruction>()
                {
                    new Say()
                    {
                        Locale = "en-US",
                        Text = "Sorry, the conference has been cancelled. The call will end now."
                    }
                }
            });
        }

        [Then(@"the update call response contains a ""(.*)"" error")]
        public async Task ThenTheUpdateCallResponseContainsAError(string p0)
        {
            // TODO: add VoiceApiException with additional fields like errorCode and reference
            await _updateNonExistentOp.Should().ThrowExactlyAsync<SinchApiException>()
                .Where(x => x.StatusCode == HttpStatusCode.NotFound && x.Message == "Not Found:Call not found");
        }

        [When(@"I send a request to manage a call with callLeg")]
        public void WhenISendARequestToManageACallWithCallLeg()
        {
            _manageWithCallLeg = () => _sinchVoiceCalls.ManageWithCallLeg("1ce0ffee-ca11-ca11-ca11-abcdef000032",
                new CallLeg("callee"), new ManageWithCallLegRequest()
                {
                    Action = new Continue(),
                    Instructions = new List<IInstruction>()
                    {
                        new PlayFiles()
                        {
                            Ids = new List<string>()
                            {
                                "https://samples-files.com/samples/Audio/mp3/sample-file-4.mp3"
                            }
                        }
                    }
                });
        }

        [Then(@"the manage a call with callLeg response contains no data")]
        public async Task ThenTheManageACallWithCallLegResponseContainsNoData()
        {
            await _manageWithCallLeg.Should().NotThrowAsync();
        }
    }
}
