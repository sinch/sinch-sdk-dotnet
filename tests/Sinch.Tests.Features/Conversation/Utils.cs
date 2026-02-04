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
                    SinchOptions = new SinchOptions
                    {
                        ApiUrlOverrides = new ApiUrlOverrides()
                        {
                            AuthUrl = "http://localhost:3011",
                            ConversationUrl = "http://localhost:3014",
                            TemplatesUrl = "http://localhost:3015"
                        }
                    }
                }
            ).Conversation;
        }
    }
}
