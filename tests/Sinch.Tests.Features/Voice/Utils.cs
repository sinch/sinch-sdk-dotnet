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
                    VoiceUrl = "http://localhost:3019",
                    VoiceApplicationManagementUrl = "http://localhost:3020"
                };
            }).Voice("appKey", "BeIukql3pTKJ8RGL5zo0DA==");
    }
}
