using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Reqnroll;
using Sinch.Voice;
using Sinch.Voice.Callouts.Callout;
using Sinch.Voice.Conferences;
using Sinch.Voice.Conferences.Get;
using Sinch.Voice.Conferences.ManageParticipants;

namespace Sinch.Tests.Features.Voice
{
    [Binding]
    public class Conferences
    {
        private ISinchVoiceConferences _sinchVoiceConferences;
        private CalloutResponse _conferencesResponse;
        private GetConferenceResponse _conferenceResponse;
        private Func<Task> _putOnHoldOp;
        private Func<Task> _kickOp;
        private Func<Task> _kickAllOp;

        [Given(@"the Voice service ""Conferences"" is available")]
        public void GivenTheVoiceServiceIsAvailable()
        {
            _sinchVoiceConferences = Utils.TestSinchVoiceClient.Conferences;
        }

        [When(@"I send a request to make a Conference call with the ""Conferences"" service")]
        public async Task WhenISendARequestToMakeAConferenceCallWithTheService()
        {
            // TODO: forward from Conferences tag
            _conferencesResponse = await Utils.TestSinchVoiceClient.Callouts.Conference(new ConferenceCalloutRequest()
            {
                Cli = "+12015555555",
                Destination = new Destination()
                {
                    Type = DestinationType.Number,
                    Endpoint = "+12017777777"
                },
                ConferenceId = "myConferenceId-E2E",
                Locale = "en-US",
                Greeting = "Welcome to this conference call.",
                MohClass = MohClass.Music1
            });
        }

        [Then(@"the callout response from the ""Conferences"" service contains the Conference call ID")]
        public void ThenTheCalloutResponseFromTheServiceContainsTheConferenceCallId()
        {
            _conferencesResponse.Should().BeEquivalentTo(new CalloutResponse()
            {
                CallId = "1ce0ffee-ca11-ca11-ca11-abcdef000002"
            });
        }

        [When(@"I send a request to get the conference information")]
        public async Task WhenISendARequestToGetTheConferenceInformation()
        {
            _conferenceResponse = await _sinchVoiceConferences.Get("myConferenceId-E2E");
        }

        [Then(@"the response contains the information about the conference participants")]
        public void ThenTheResponseContainsTheInformationAboutTheConferenceParticipants()
        {
            _conferenceResponse.Should().BeEquivalentTo(new GetConferenceResponse()
            {
                Participants = new List<Participant>()
                {
                    new Participant
                    {
                        Cli = "+12015555555",
                        Id = "1ce0ffee-ca11-ca11-ca11-abcdef000012",
                        Duration = 35,
                        Muted = true,
                        OnHold = true
                    },
                    new Participant
                    {
                        Cli = "+12015555555",
                        Id = "1ce0ffee-ca11-ca11-ca11-abcdef000022",
                        Duration = 6,
                        Muted = false,
                        OnHold = false
                    }
                }
            });
        }

        [When(@"I send a request to put a participant on hold")]
        public void WhenISendARequestToPutAParticipantOnHold()
        {
            _putOnHoldOp = () => _sinchVoiceConferences.ManageParticipant("1ce0ffee-ca11-ca11-ca11-abcdef000012",
                "myConferenceId-E2E", new ManageParticipantRequest()
                {
                    Moh = MohClass.Music2,
                    Command = Command.OnHold
                });
        }

        [Then(@"the manage participant response contains no data")]
        public async Task ThenTheManageParticipantResponseContainsNoData()
        {
            await _putOnHoldOp.Should().NotThrowAsync();
        }

        [When(@"I send a request to kick a participant from a conference")]
        public void WhenISendARequestToKickAParticipantFromAConference()
        {
            _kickOp = () =>
                _sinchVoiceConferences.KickParticipant("1ce0ffee-ca11-ca11-ca11-abcdef000012", "myConferenceId-E2E");
        }

        [Then(@"the kick participant response contains no data")]
        public async Task ThenTheKickParticipantResponseContainsNoData()
        {
            await _kickOp.Should().NotThrowAsync();
        }

        [When(@"I send a request to kick all the participants from a conference")]
        public void WhenISendARequestToKickAllTheParticipantsFromAConference()
        {
            _kickAllOp = () => _sinchVoiceConferences.KickAll("myConferenceId-E2E");
        }

        [Then(@"the kick all participants response contains no data")]
        public async Task ThenTheKickAllParticipantsResponseContainsNoData()
        {
            await _kickAllOp.Should().NotThrowAsync();
        }
    }
}
