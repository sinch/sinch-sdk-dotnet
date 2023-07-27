using System;
using Moq;
using Sinch.Auth;

namespace Sinch.Tests.e2e
{
    public class TestBase
    {
        private const string ProjectId = "e15b2651-daac-4ccb-92e8-e3066d1d033b";
        protected readonly ISinch _sinchClient;

        protected TestBase()
        {
            _sinchClient = new Sinch.SinchClient(ProjectId, new Uri("http://localhost:8001"),
                new Uri("http://localhost:8000"));
        }
    }
}
