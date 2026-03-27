using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Contacts.List;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

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

Console.WriteLine("Listing all conversation contacts (page by page)");

ListContactsResponse response = null;
do
{
    response = await client.Conversation.Contacts.List(new ListContactsRequest
    {
        PageToken = response?.NextPageToken
    });

    foreach (var contact in response.Contacts ?? [])
        Console.WriteLine(contact.ToPrettyString());

} while (!string.IsNullOrEmpty(response.NextPageToken));
