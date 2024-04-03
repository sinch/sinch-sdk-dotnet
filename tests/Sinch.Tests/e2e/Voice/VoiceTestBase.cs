using Sinch.Auth;
using Sinch.Voice;

namespace Sinch.Tests.e2e.Voice
{
    public class VoiceTestBase : TestBase
    {
        protected readonly ISinchVoiceClient VoiceClient;

        protected VoiceTestBase()
        {
            VoiceClient = SinchClientMockServer.Voice("app_key", "app_secret", default, AuthStrategy.Basic);
        }
    }
}
