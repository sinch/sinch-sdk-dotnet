using Sinch.Voice;

namespace Sinch.Tests.Features.Voice
{
    public class Utils
    {
        public static SinchClient TestSinchClient => new SinchClient(new SinchClientConfiguration()
        {
            VoiceConfiguration = new SinchVoiceConfiguration()
            {
                AppKey = "appKey",
                AppSecret = "BeIukql3pTKJ8RGL5zo0DA==",
                VoiceUrlOverride = "http://localhost:3019",
                ApplicationManagementUrlOverride = "http://localhost:3020",
            }
        });
    }
}
