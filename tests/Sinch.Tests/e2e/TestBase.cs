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
            SinchClientMockStudio = new SinchClient(ProjectId, "key_id", "key_secret",
                options =>
                {
                    options.ApiUrlOverrides = new ApiUrlOverrides()
                    {
                        AuthUrl = "http://localhost:8001",
                        NumbersUrl = "http://localhost:8000",
                        SmsUrl = "http://localhost:8002"
                    };
                });

            SinchClientMockServer = new SinchClient(ProjectId, "key_id", "key_secret", options =>
            {
                options.ApiUrlOverrides = new ApiUrlOverrides()
                {
                    AuthUrl = GetTestUrl("MOCK_AUTH_PORT"),
                    SmsUrl = GetTestUrl("MOCK_SMS_PORT"),
                    ConversationUrl = GetTestUrl("MOCK_CONVERSATION_PORT"),
                    NumbersUrl = GetTestUrl("MOCK_NUMBERS_PORT"),
                    VoiceUrl = GetTestUrl("MOCK_VOICE_PORT"),
                    VerificationUrl = GetTestUrl("MOCK_VERIFICATION_PORT"),
                    // templates treated as conversation api in doppelganger 
                    TemplatesUrl = GetTestUrl("MOCK_CONVERSATION_PORT"),
                    FaxUrl = GetTestUrl("MOCK_FAX_PORT"),
                };
            });
        }

        private string GetTestUrl(string portEnvVar) =>
            $"http://localhost:{Environment.GetEnvironmentVariable(portEnvVar)}";
    }
}
