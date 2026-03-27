using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts.GetChannelProfile;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// Conversation app ID used to look up the channel profile
const string appId = "CONVERSATION_APP_ID";
// The Messenger user IDs to look up
const string messengerUserId1 = "MESSENGER_USER_ID_1";
const string messengerUserId2 = "MESSENGER_USER_ID_2";

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

Console.WriteLine("Getting Messenger channel profile by channel identities");

var response = await client.Conversation.Contacts.GetChannelProfileByChannelIdentity(
    appId,
    ChannelProfileConversationChannel.Messenger,
    [
        new ChannelIdentity { Channel = ConversationChannel.Messenger, Identity = messengerUserId1 },
        new ChannelIdentity { Channel = ConversationChannel.Messenger, Identity = messengerUserId2 }
    ]);

Console.WriteLine($"Profile name: {response.ProfileName}");

