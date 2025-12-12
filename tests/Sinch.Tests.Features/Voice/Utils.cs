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
                            VoiceUrl = "http://localhost:3019",
                            VoiceApplicationManagementUrl = "http://localhost:3020"
                        }
                    },
                    VoiceConfiguration = new SinchVoiceConfiguration()
                    {
                        AppKey = "appKey",
                        AppSecret = "keyId"
                    }
                }
            ).Voice;
    }
}
