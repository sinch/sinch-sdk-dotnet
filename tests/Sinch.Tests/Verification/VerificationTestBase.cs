using System;
using Sinch.Verification;

namespace Sinch.Tests.Verification
{
    public class VerificationTestBase : TestBase
    {
        internal readonly ISinchVerification Verification;

        protected VerificationTestBase()
        {
            Verification = new SinchVerification(default, new Uri("https://verification.api.sinch.com"), HttpCamelCase);
        }
    }
}
