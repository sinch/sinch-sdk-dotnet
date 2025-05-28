using Sinch.Auth;
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
                    VerificationUrl = "http://localhost:3018"
                };
            }).Verification("appKey", "appSecret", AuthStrategy.Basic);
    }
}
