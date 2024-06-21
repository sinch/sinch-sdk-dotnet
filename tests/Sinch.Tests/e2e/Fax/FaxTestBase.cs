using Sinch.Fax;

namespace Sinch.Tests.e2e.Fax
{
    public class FaxTestBase : TestBase
    {
        protected readonly ISinchFax FaxClient;

        protected FaxTestBase()
        {
            FaxClient = SinchClientMockServer.Fax;
        }
    }
}
