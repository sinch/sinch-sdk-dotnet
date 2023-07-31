using System;

namespace Sinch.Tests.e2e
{
    public class TestBase
    {
        private const string ProjectId = "e15b2651-daac-4ccb-92e8-e3066d1d033b";
        protected readonly ISinch SinchClient;

        protected TestBase()
        {
            SinchClient = new Sinch.SinchClient(ProjectId, new Uri("http://localhost:8001"),
                new Uri("http://localhost:8000"),
                new Uri("http://localhost:8002"));
        }
    }
}
