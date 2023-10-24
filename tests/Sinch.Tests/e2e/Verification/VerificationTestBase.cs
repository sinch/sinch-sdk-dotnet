using Sinch.Verification;

namespace Sinch.Tests.e2e.Verification
{
    public class VerificationTestBase : TestBase
    {
        protected readonly ISinchVerificationClient VerificationClient;

        public VerificationTestBase()
        {
            VerificationClient = SinchClientMockServer.Verification("app_key", "app_secret", AuthStrategy.Basic);
        }
    }
}
