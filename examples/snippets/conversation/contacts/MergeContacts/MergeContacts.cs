using Sinch;
using Sinch.Conversation;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// The destination contact (the one that is kept after merging)
const string destinationContactId = "DESTINATION_CONTACT_ID";
// The source contact (the one that is removed after merging)
const string sourceContactId = "SOURCE_CONTACT_ID";

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

Console.WriteLine($"Merging contact '{sourceContactId}' into '{destinationContactId}'");

var response = await client.Conversation.Contacts.Merge(destinationContactId, sourceContactId);

Console.WriteLine($"Response: {response.ToPrettyString()}");
