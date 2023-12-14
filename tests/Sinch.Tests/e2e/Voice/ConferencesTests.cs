using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Voice;
using Sinch.Voice.Conferences.Get;
using Sinch.Voice.Conferences.ManageParticipants;
using Xunit;

namespace Sinch.Tests.e2e.Voice
{
    public class ConferencesTests : VoiceTestBase
    {
        [Fact]
        public async Task KickAll()
        {
            var s = () => VoiceClient.Conferences.KickAll("123");
            await s.Should().NotThrowAsync();
        }

        [Fact]
        public async Task KickAllNotFound()
        {
            var s = () => VoiceClient.Conferences.KickAll("456");
            await s.Should().ThrowAsync<SinchApiException>()
                .Where(x => x.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetConference()
        {
            var response = await VoiceClient.Conferences.Get("1");
            response.Should().BeEquivalentTo(new GetConferenceResponse
            {
                Participants = new List<Participant>
                {
                    new Participant
                    {
                        Cli = "123",
                        Duration = 15,
                        Id = "call1",
                        Muted = false,
                        Onhold = false
                    },
                    new Participant
                    {
                        Cli = "345",
                        Duration = 70,
                        Id = "call2",
                        Muted = true,
                        Onhold = true
                    }
                }
            });
        }

        [Fact]
        public async Task ManageParticipant()
        {
            var op = () => VoiceClient.Conferences.ManageParticipant("1", "conf1", new ManageParticipantRequest
            {
                Command = Command.Mute,
                Moh = MohClass.Music2
            });
            await op.Should().NotThrowAsync();
        }
        
        [Fact]
        public async Task KickParticipant()
        {
            var op = () => VoiceClient.Conferences.KickParticipant("1", "conf1");
            await op.Should().NotThrowAsync();
        }
    }
}
