using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Messages.List;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// The ID of the Conversation Application
const string conversationApplicationId = "CONVERSATION_APP_ID";

// Channel identity to filter by
const string channelIdentity = "CHANNEL_IDENTITY";

var client = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    },
    ConversationConfiguration = new SinchConversationConfiguration
    {
        ConversationRegion = new ConversationRegion(conversationRegion)
    }
});

var request = new ListMessagesByChannelIdentityRequest
{
    AppId = conversationApplicationId,
    ChannelIdentities = new List<string> { channelIdentity }
};

Console.WriteLine($"Listing messages by channel identity for application '{conversationApplicationId}'");

var response = await client.Conversation.Messages.ListMessagesByChannelIdentity(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
