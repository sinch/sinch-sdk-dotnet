using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// The ID of the Conversation App where the recipient channel is configured
const string applicationId = "CONVERSATION_APP_ID";
// The contact ID of the recipient to look up the capabilities for
const string contactId = "RECIPIENT_CONTACT_ID";

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

// Lookup capability for a contact that already exists in the Conversation API.
var response = await client.Conversation.Capabilities.Lookup(new LookupCapabilityRequest
{
    AppId = applicationId,
    Recipient = new ContactRecipient
    {
        ContactId = contactId
    }
});

Console.WriteLine($"Response: {response.ToPrettyString()}");
