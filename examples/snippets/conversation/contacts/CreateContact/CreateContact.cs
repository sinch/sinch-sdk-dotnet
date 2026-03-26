using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts.Create;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// Phone number of the contact in E.164 format
const string recipientPhoneNumber = "RECIPIENT_PHONE_NUMBER";
// Language of the contact
const string contactLanguage = "CONTACT_LANGUAGE";
// Display name of the contact
const string contactDisplayName = "CONTACT_DISPLAY_NAME";

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

Console.WriteLine("Creating conversation contact");

var contactRequest = new CreateContactRequest
{
    ChannelIdentities = [new ChannelIdentity { Channel = ConversationChannel.Sms, Identity = recipientPhoneNumber }],
    Language = contactLanguage,
    DisplayName = contactDisplayName
};

var response = await client.Conversation.Contacts.Create(contactRequest);

Console.WriteLine($"Response: {response.ToPrettyString()}");
