using Sinch.Voice;

namespace Sinch.Tests.Features.Voice
{
    public class Utils
    {
        public static ISinchVoiceClient TestSinchVoiceClient =>
            new SinchClient(
                new SinchClientConfiguration
                {
                    SinchOptions = new SinchOptions
                    {
                        ApiUrlOverrides = new ApiUrlOverrides()
                        {
                            VoiceUrl = Helpers.MOCKSERVER_VOICE_URL,
                            VoiceApplicationManagementUrl = Helpers.MOCKSERVER_VOICE_APPLICATION_MANAGEMENT_URL
                        }
                    },
                    VoiceConfiguration = new SinchVoiceConfiguration()
                    {
                        AppKey = "appKey",
                        AppSecret = "BeIukql3pTKJ8RGL5zo0DA=="
                    }
                }
            ).Voice;
    }
}
