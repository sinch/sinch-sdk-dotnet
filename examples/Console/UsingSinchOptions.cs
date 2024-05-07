using Sinch;

namespace Examples;

public class UsingSinchOptions
{
    public void Example()
    {
        var sinch = new SinchClient(Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
            Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
            Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!,
            options =>
            {
                options.SmsRegion = Sinch.SMS.SmsRegion.Eu;
                options.ConversationRegion = Sinch.Conversation.ConversationRegion.Eu;
            });
    }
}
