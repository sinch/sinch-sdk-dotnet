using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Contacts;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// Contact ID to update
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

Console.WriteLine($"Updating conversation contact '{contactId}'");

var contact = new Contact
{
    Id = contactId,
    DisplayName = "Updated name with the .NET SDK",
    ChannelPriority = [ConversationChannel.Messenger]
};

var response = await client.Conversation.Contacts.Update(contact);

Console.WriteLine($"Response: {response.ToPrettyString()}");
