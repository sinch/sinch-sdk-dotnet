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
                    VerificationUrl = "http://localhost:3018",
                    AuthUrl = "http://localhost:3011",
                };
            }).Verification("appKey", "YXBwU2VjcmV0");
    }
}
