using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;
using Sinch.Core;
using Sinch.Snippets.Shared;

var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    },
    ConversationConfiguration = new SinchConversationConfiguration
    {
        ConversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION"
    }
});

// The ID of the Conversation App where the recipient channel is configured
const string applicationId = "CONVERSATION_APP_ID";
// The contact ID of the recipient to look up the capabilities for
const string contactId = "RECIPIENT_CONTACT_ID";

// Lookup capability for a contact that already exists in the Conversation API.
var response = await sinch.Conversation.Capabilities.Lookup(new LookupCapabilityRequest
{
    AppId = applicationId,
    Recipient = new ContactRecipient
    {
        ContactId = contactId
    }
});

Console.WriteLine($"Response: {response.ToPrettyString()}");
