using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts.GetChannelProfile;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// Conversation app ID used to look up the channel profile
const string appId = "CONVERSATION_APP_ID";
// The Messenger user ID of the contact
const string messengerUserId = "MESSENGER_USER_ID";

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

Console.WriteLine($"Getting Messenger channel profile for identity '{messengerUserId}'");

var response = await client.Conversation.Contacts.GetChannelProfileByChannelIdentity(
    appId,
    ChannelProfileConversationChannel.Messenger,
    new ChannelIdentity { Channel = ConversationChannel.Messenger, Identity = messengerUserId });

Console.WriteLine($"Profile name: {response.ProfileName}");
