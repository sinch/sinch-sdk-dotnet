using Sinch.Verification;

namespace Sinch.Tests.Features.Verification
{
    public class Utils
    {
        public static ISinchVerificationClient SinchVerificationClient =>
        new SinchClient(
                new SinchClientConfiguration()
                {
                    VerificationConfiguration = new SinchVerificationConfiguration()
                    {
                        AppKey = "appKey",
                        AppSecret = "YXBwU2VjcmV0"
                    },
                    SinchOptions = new SinchOptions
                    {
                        ApiUrlOverrides = new ApiUrlOverrides()
                        {
                            AuthUrl = "http://localhost:3011",
                            VerificationUrl = "http://localhost:3011"
                        }
                    }
                }
            ).Verification;
    }
}
