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
            var client = new SinchClient(new SinchClientConfiguration()
            {
                SinchCommonCredentials = new SinchCommonCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                }
            });
            var voiceClient = client.Voice("key", "secret");
            var baseUrl = Helpers.GetPrivateField<Uri, ISinchVoiceCallout>(voiceClient.Callouts, "_baseAddress");
            baseUrl.Should().BeEquivalentTo(new Uri("https://calling.api.sinch.com/"));
        }

        [Fact]
        public void InitVoiceWithEastAsia1Region()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                SinchCommonCredentials = new SinchCommonCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                }
            });
            var voiceClient = client.Voice("key", "secret", VoiceRegion.SouthEastAsia1);
            var baseUrl = Helpers.GetPrivateField<Uri, ISinchVoiceCallout>(voiceClient.Callouts, "_baseAddress");
            baseUrl.Should().BeEquivalentTo(new Uri("https://calling-apse1.api.sinch.com/"));
        }
    }
}
