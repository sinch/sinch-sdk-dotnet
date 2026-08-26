using Sinch.Voice;

namespace Sinch.Tests.Features.Voice
{
    public class Utils
    {
        public static ISinchVoiceClient TestSinchVoiceClient =>
            new SinchClient(null, null, null, options =>
            {
                options.ApiUrlOverrides = new ApiUrlOverrides()
                {
                    VoiceUrl = Helpers.MOCKSERVER_VOICE_URL,
                    VoiceApplicationManagementUrl = Helpers.MOCKSERVER_VOICE_APPLICATION_MANAGEMENT_URL
                };
            }).Voice("appKey", "BeIukql3pTKJ8RGL5zo0DA==");
    }
}
