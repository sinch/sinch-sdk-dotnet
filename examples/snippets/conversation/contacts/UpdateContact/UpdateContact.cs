using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// Contact ID to update
const string contactId = "CONTACT_ID";
// Recipient phone number in E.164 format
const string recipientPhoneNumber = "RECIPIENT_PHONE_NUMBER";

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

// Update the channel identities and priority on an existing contact
Console.WriteLine($"Updating conversation contact '{contactId}'");

var contact = new Contact
{
    Id = contactId,
    ChannelIdentities =
    [
        new() { Channel = ConversationChannel.Sms, Identity = recipientPhoneNumber }
    ],
    ChannelPriority = [ConversationChannel.Sms]
};

var response = await client.Conversation.Contacts.Update(contact);

Console.WriteLine($"Response: {response.ToPrettyString()}");
