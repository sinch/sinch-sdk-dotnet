using System;
using DotNetEnv;

namespace Sinch.Tests.e2e
{
    public class TestBase
    {
        /// <summary>
        ///     It's the same value as in doppleganger common.defaultProjectId, so it's shared and common. 
        /// </summary>
        private const string ProjectId = "e15b2651-daac-4ccb-92e8-e3066d1d033b";

        protected readonly ISinchClient SinchClientMockStudio;

        // MockStudio should be removed and all contract testing should go to mock server version
        protected readonly ISinchClient SinchClientMockServer;

        protected TestBase()
        {
            Env.Load();
            SinchClientMockStudio = new SinchClient(ProjectId, new Uri("http://localhost:8001"),
                new Uri("http://localhost:8000"),
                new Uri("http://localhost:8002"), null, null, null);
            var authUri = new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_AUTH_PORT")}");
            var smsUri = new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_SMS_PORT")}");
            // TODO: use mock server endpoints
            var numbersUri = new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_NUMBERS_PORT")}");
            var verificationBaseUri =
                new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_VERIFICATION_PORT")}");
            var voiceBaseUri =
                new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_VOICE_PORT")}");
            var conversationBaseUri =
                new Uri($"http://localhost:{Environment.GetEnvironmentVariable("MOCK_CONVERSATION_PORT")}");
            SinchClientMockServer = new SinchClient(ProjectId, authUri,
                new Uri("http://localhost:8000"),
                smsUri, verificationBaseUri, voiceBaseUri, conversationBaseUri);
        }
    }
}
