using System;
using Sinch.Verification;

namespace Sinch.Tests.Verification
{
    public class VerificationTestBase : TestBase
    {
        internal readonly ISinchVerificationClient VerificationClient;
        protected VerificationTestBase()
        {
            // VerificationClient = new SinchVerificationClient("app_key", "app_secret", new Uri("https://"))
        }
    }
}
