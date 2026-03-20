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
// The channel identities to list messages for (e.g. recipient phone numbers for SMS)
List<string> channelIdentities = ["CHANNEL_IDENTITY1", "CHANNEL_IDENTITY2"];

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
        Region = new ConversationRegion(conversationRegion)
    }
});

var request = new ListMessagesByChannelIdentityRequest
{
    AppId = conversationApplicationId,
    Channel = ConversationChannel.Sms,
    ChannelIdentities = channelIdentities,
    MessagesSource = MessageSource.ConversationSource
};

Console.WriteLine($"Listing SMS messages for '{string.Join(",", channelIdentities)}' in application '{conversationApplicationId}'");

var response = await client.Conversation.Messages.ListLastMessagesByChannelIdentity(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
