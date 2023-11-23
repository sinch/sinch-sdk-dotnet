using System;
using FluentAssertions;
using Sinch.Voice;
using Sinch.Voice.Callouts;
using Xunit;

namespace Sinch.Tests.Voice
{
    public class VoiceClientTests
    {
        [Fact]
        public void InitVoiceWithGlobalRegion()
        {
            var client = new SinchClient("id", "secret", "id");
            var voiceClient = client.Voice("key", "secret");
            var baseUrl = Helpers.GetPrivateField<Uri, ISinchVoiceCallout>(voiceClient.Callouts, "_baseAddress");
            baseUrl.Should().BeEquivalentTo(new Uri("https://calling.api.sinch.com/"));
        }
        
        [Fact]
        public void InitVoiceWithEastAsia1Region()
        {
            var client = new SinchClient("id", "secret", "id");
            var voiceClient = client.Voice("key", "secret", CallingRegion.SouthEastAsia1);
            var baseUrl = Helpers.GetPrivateField<Uri, ISinchVoiceCallout>(voiceClient.Callouts, "_baseAddress");
            baseUrl.Should().BeEquivalentTo(new Uri("https://calling-apse1.api.sinch.com/"));
        }
    }
}
