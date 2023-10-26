using System;
using Sinch.Verification;

namespace Sinch.Tests.Verification
{
    public class VerificationTestBase : TestBase
    {
        internal readonly ISinchVerificationClient VerificationClient;

        protected VerificationTestBase()
        {
            VerificationClient = new SinchVerificationClient(
                new Uri("https://verification.api.sinch.com/"), null, HttpCamelCase);
        }
    }
}
