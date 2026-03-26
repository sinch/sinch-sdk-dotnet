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
// Contact ID whose channel profile to retrieve
const string contactId = "CONTACT_ID";

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

Console.WriteLine($"Getting channel profile for contact '{contactId}' on Messenger");

var request = new GetChannelProfileRequest
{
    AppId = appId,
    Recipient = new ContactRecipient { ContactId = contactId },
    Channel = ChannelProfileConversationChannel.Messenger
};

var response = await client.Conversation.Contacts.GetChannelProfile(request);

Console.WriteLine($"Profile name: {response.ProfileName}");