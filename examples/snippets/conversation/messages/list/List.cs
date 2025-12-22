using System.Text.Json;
using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Messages.List;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID") ?? "MY_PROJECT_ID",
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID") ?? "MY_KEY_ID",
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET") ?? "MY_KEY_SECRET",
    },
    ConversationConfiguration = new SinchConversationConfiguration
    {
        ConversationRegion = new ConversationRegion(Environment.GetEnvironmentVariable("SINCH_CONVERSATION_REGION") ?? "MY_CONVERSATION_REGION")
            
    }
});

var messagesService = sinchClient.Conversation.Messages;

var request = new ListMessagesRequest
{
    MessagesSource = MessageSource.ConversationSource
};

Console.WriteLine("Get messages list");

var response = await messagesService.List(request);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
