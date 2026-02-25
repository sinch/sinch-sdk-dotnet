using Sinch;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;
using Sinch.Core;
using Sinch.Snippets.Shared;

// Your conversation application id
const string applicationId = "MY_APPLICATION_ID";
// Your conversation contact id
const string contactId = "MY_CONTACT_ID";

var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

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
