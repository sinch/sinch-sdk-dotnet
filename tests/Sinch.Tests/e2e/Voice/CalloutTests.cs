using System.Threading.Tasks;
using FluentAssertions;
using Sinch.Voice.Callouts.Callout;
using Xunit;

namespace Sinch.Tests.e2e.Voice
{
    public class CalloutTests : VoiceTestBase
    {
        [Fact]
        public async Task TtsRequest()
        {
            var response = await VoiceClient.Callouts.Tts(new TtsCalloutRequest()
            {
                Destination = new Destination()
                {
                    Endpoint = "+14045005000",
                    Type = DestinationType.Number,
                },
                Cli = "+14045001000",
                Dtmf = "w9",
                Domain = Domain.Mxp,
                Custom = "opaque",
                Locale = "en-US",
                Text = "Hello, this is a call from Sinch.",
                Promts = "#tts[Hello from Sinch]",
                EnableAce = true,
                EnableDice = true,
                EnablePie = true,
            });
            response.CallId.Should().BeEquivalentTo("220");
        }

        [Fact]
        public async Task ConferenceRequest()
        {
            var response = await VoiceClient.Callouts.Conference(new ConferenceCalloutRequest()
            {
                Destination = new Destination()
                {
                    Endpoint = "+2022",
                    Type = DestinationType.Username,
                },
                ConferenceId = "r1lf9s32sa",
                Cli = "+3033",
                ConferenceDtmfOptions = new ConferenceDtmfOptions()
                {
                    Mode = DtmfMode.Forward,
                    MaxDigits = null,
                    TimeoutMills = 300
                },
                Dtmf = "w9",
                MaxDuration = 12000,
                EnableAce = true,
                EnableDice = false,
                EnablePie = null,
                Locale = "en-US",
                Greeting = "konichiwa",
                MohClass = MohClass.Music3,
                Custom = "arigato",
                Domain = Domain.Pstn
            });
            response.CallId.Should().BeEquivalentTo("330");
        }

        [Fact]
        public async Task CustomRequest()
        {
            var response = await VoiceClient.Callouts.Custom(new CustomCalloutRequest()
            {
                Cli = "+3033",
                Destination = new Destination()
                {
                    Endpoint = "+2022",
                    Type = DestinationType.Username,
                },
                Dtmf = "w9",
                Custom = "arigato",
                MaxDuration = 300,
                Ice = "{\"action\":{\"name\":\"connectPstn\",\"number\":\"46000000001\",\"maxDuration\":90}}",
                Ace = "{}",
                Pie = "https://your-application-server-host/application"
            });
            response.CallId.Should().BeEquivalentTo("440");
        }
    }
}
