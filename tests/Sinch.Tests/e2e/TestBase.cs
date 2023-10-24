using System;
using DotNetEnv;

namespace Sinch.Tests.e2e
{
    public class TestBase
    {
        private const string ProjectId = "e15b2651-daac-4ccb-92e8-e3066d1d033b";

        protected readonly ISinch SinchClientMockStudio;

        // MockStudio should be removed and all contract testing should go to mock server version
        protected readonly ISinch SinchClientMockServer;

        protected TestBase()
        {
            Env.Load();
            SinchClientMockStudio = new Sinch.SinchClient(ProjectId, new Uri("http://localhost:8001"),
                new Uri("http://localhost:8000"),
                new Uri("http://localhost:8002"), null);
            var authUri = new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_AUTH_PORT")}");
            var smsUri = new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_SMS_PORT")}");
            // TODO: use mock server endpoints
            var numbersUri = new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_NUMBERS_PORT")}");
            var verificationBaseUri =
                new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_VERIFICATION_PORT")}");
            SinchClientMockServer = new Sinch.SinchClient(ProjectId, authUri,
                new Uri("http://localhost:8000"),
                smsUri, verificationBaseUri);
        }
    }
}
