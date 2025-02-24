using System;
using DotNetEnv;
using Sinch.Auth;
using Sinch.Conversation;
using Sinch.Fax;
using Sinch.Numbers;
using Sinch.SMS;
using Sinch.Verification;

namespace Sinch.Tests.e2e
{
    public class TestBase
    {
        /// <summary>
        ///     It's the same value as in doppleganger common.defaultProjectId, so it's shared and common. 
        /// </summary>
        protected const string ProjectId = "e15b2651-daac-4ccb-92e8-e3066d1d033b";

        protected readonly ISinchClient SinchClientMockStudio;

        // MockStudio should be removed and all contract testing should go to mock server version
        protected readonly ISinchClient SinchClientMockServer;

        protected readonly string WebhooksEventsBaseAddress;

        protected TestBase()
        {
            Env.Load();
            SinchClientMockStudio = new SinchClient(new SinchClientConfiguration()
            {
                SinchCommonCredentials = new SinchCommonCredentials()
                {
                    ProjectId = ProjectId,
                    KeyId = "key_id",
                    KeySecret = "key_secret",
                },
                OAuthConfiguration = new OAuthConfiguration()
                {
                    UrlOverride = "http://localhost:8001"
                },
                NumbersConfiguration = new SinchNumbersConfiguration()
                {
                    UrlOverride = "http://localhost:8000",
                },
                SmsConfiguration = new SinchSmsConfiguration()
                {
                    UrlOverride = "http://localhost:8002",
                }
            });
            var conversationUrl = GetTestUrl("MOCK_CONVERSATION_PORT");

            SinchClientMockServer = new SinchClient(new SinchClientConfiguration()
            {
                SinchCommonCredentials = new SinchCommonCredentials()
                {
                    ProjectId = ProjectId,
                    KeyId = "key_id",
                    KeySecret = "key_secret",
                },
                OAuthConfiguration = new OAuthConfiguration()
                {
                    UrlOverride = GetTestUrl("MOCK_AUTH_PORT"),
                },
                SmsConfiguration = new SinchSmsConfiguration()
                {
                    UrlOverride = GetTestUrl("MOCK_SMS_PORT"),
                },
                ConversationConfiguration = new SinchConversationConfiguration()
                {
                    ConversationUrlOverride = conversationUrl,
                    TemplateUrlOverride = conversationUrl,
                },
                NumbersConfiguration = new SinchNumbersConfiguration()
                {
                    UrlOverride = GetTestUrl("MOCK_NUMBERS_PORT"),
                },
                FaxConfiguration = new SinchFaxConfiguration()
                {
                    UrlOverride = GetTestUrl("MOCK_FAX_PORT"),
                },
                VerificationConfiguration = new SinchVerificationConfiguration()
                {
                    AppKey = "app_key",
                    AppSecret = "app_secret",
                    AuthStrategy = AuthStrategy.Basic, // only for e2e tests, not visible in public API
                    UrlOverride = GetTestUrl("MOCK_VERIFICATION_PORT"),
                }
            });
            // SinchClientMockServer = new SinchClient(ProjectId, "key_id", "key_secret", options =>
            // {
            //     options.ApiUrlOverrides = new ApiUrlOverrides()
            //     {
            //         AuthUrl = GetTestUrl("MOCK_AUTH_PORT"),
            //         SmsUrl = GetTestUrl("MOCK_SMS_PORT"),
            //         ConversationUrl = conversationUrl,
            //         NumbersUrl = GetTestUrl("MOCK_NUMBERS_PORT"),
            //         VoiceUrl = GetTestUrl("MOCK_VOICE_PORT"),
            //         // Voice Application Management treated the same as voice in doppelganger
            //         VoiceApplicationManagementUrl = GetTestUrl("MOCK_VOICE_PORT"),
            //         VerificationUrl = GetTestUrl("MOCK_VERIFICATION_PORT"),
            //         // templates treated as conversation api in doppelganger 
            //         FaxUrl = GetTestUrl("MOCK_FAX_PORT"),
            //         TemplatesUrl = conversationUrl,
            //     };
            // });
            WebhooksEventsBaseAddress = conversationUrl + "/webhooks/conversation/";
        }

        private string GetTestUrl(string portEnvVar) =>
            $"http://localhost:{Environment.GetEnvironmentVariable(portEnvVar)}";
    }
}
