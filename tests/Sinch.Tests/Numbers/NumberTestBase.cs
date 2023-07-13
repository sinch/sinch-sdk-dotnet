using System;
using Sinch.Numbers;

namespace Sinch.Tests.Numbers
{
    public class NumberTestBase : TestBase
    {
        internal readonly INumbers Numbers;

        protected NumberTestBase()
        {
            Numbers = new Sinch.Numbers.Numbers(ProjectId,
                new Uri("https://numbers.api.sinch.com/"), default, HttpCamelCase);
        }
    }
}
