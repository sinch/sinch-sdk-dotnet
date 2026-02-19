using System;
using Sinch.Fax;

namespace Sinch.Tests.Fax
{
    public class FaxTestBase : TestBase
    {
        internal readonly ISinchFax Fax;

        protected FaxTestBase()
        {
            Fax = new FaxClient(ProjectId, new Uri("https://fax.api.sinch.com/"), default, HttpCamelCase);
        }
    }
}
