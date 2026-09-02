using Sinch.Conversation;

namespace Sinch.Tests.Features.Conversation
{
    public class Utils
    {
        public static ISinchConversation SinchConversationClient()
        {
            return new SinchClient(
                new SinchClientConfiguration()
                {
                    SinchUnifiedCredentials = new SinchUnifiedCredentials
                    {
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        KeyId = "keyId",
                        KeySecret = "keySecret"
                    },
                    ConversationConfiguration = new SinchConversationConfiguration
                    {
                        Region = ConversationRegion.Us
                    },
                    SinchOptions = new SinchOptions
                    {
                        ApiUrlOverrides = new ApiUrlOverrides()
                        {
                            AuthUrl = Helpers.MOCKSERVER_AUTH_URL,
                            ConversationUrl = Helpers.MOCKSERVER_CONVERSATION_URL,
                            TemplatesUrl = Helpers.MOCKSERVER_CONVERSATION_TEMPLATES_URL
                        }
                    }
                }
            ).Conversation;
        }
    }
}
