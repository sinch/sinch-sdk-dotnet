using Sinch.Auth;
using Sinch.Voice;

namespace Sinch.Tests.e2e.Voice
{
    public class VoiceTestBase : TestBase
    {
        protected readonly ISinchVoiceClient VoiceClient;

        protected VoiceTestBase()
        {
            VoiceClient = SinchClientMockServer.Voice("669E367E-6BBA-48AB-AF15-266871C28135", "BeIukql3pTKJ8RGL5zo0DA==", default);
        }
    }
}
