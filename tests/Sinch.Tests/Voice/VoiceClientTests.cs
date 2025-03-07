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
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                VoiceConfiguration = new SinchVoiceConfiguration()
                {
                    AppKey = "key",
                    AppSecret = "secret",
                }
            });
            var baseUrl = Helpers.GetPrivateField<Uri, ISinchVoiceCallout>(client.Voice.Callouts, "_baseAddress");
            baseUrl.Should().BeEquivalentTo(new Uri("https://calling.api.sinch.com/"));
        }

        [Fact]
        public void InitVoiceWithEastAsia1Region()
        {
            var client = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                },
                VoiceConfiguration = new SinchVoiceConfiguration()
                {
                    AppKey = "key",
                    AppSecret = "secret",
                    Region = VoiceRegion.SouthEastAsia1
                }
            });
            var voiceClient = client.Voice;
            var baseUrl = Helpers.GetPrivateField<Uri, ISinchVoiceCallout>(voiceClient.Callouts, "_baseAddress");
            baseUrl.Should().BeEquivalentTo(new Uri("https://calling-apse1.api.sinch.com/"));
        }
    }
}
