using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Messages.List;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";

// The ID of the Conversation Application to list messages for
const string conversationApplicationId = "APPLICATION_ID";

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
        ConversationRegion = new ConversationRegion(conversationRegion)
    }
});

var conversationMessages = client.Conversation.Messages;

var request = new ListMessagesRequest
{
    AppId = conversationApplicationId,
    MessagesSource = MessageSource.ConversationSource
};

Console.WriteLine($"List messages for application with ID '{conversationApplicationId}'");

var response = await conversationMessages.List(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");

