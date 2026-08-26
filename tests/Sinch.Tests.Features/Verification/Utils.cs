using Sinch.Verification;

namespace Sinch.Tests.Features.Verification
{
    public class Utils
    {
        public static ISinchVerificationClient SinchVerificationClient =>
            new SinchClient(null, null, null, options =>
            {
                options.ApiUrlOverrides = new ApiUrlOverrides()
                {
                    VerificationUrl = Helpers.MOCKSERVER_VERIFICATION_URL,
                    AuthUrl = Helpers.MOCKSERVER_AUTH_URL,
                };
            }).Verification("appKey", "YXBwU2VjcmV0");
    }
}
