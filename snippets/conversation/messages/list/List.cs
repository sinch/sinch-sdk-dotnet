using System.Text.Json;
using Sinch;
using Sinch.Core;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.List;
using Sinch.Conversation.Messages.Message;

var projectId = Environment.GetEnvironmentVariable("MY_PROJECT_ID");
var keyId = Environment.GetEnvironmentVariable("MY_KEY_ID");
var keySecret = Environment.GetEnvironmentVariable("MY_KEY_SECRET");
var conversationRegion = Environment.GetEnvironmentVariable("MY_CONVERSATION_REGION");

var messagesService = new SinchClient(projectId, keyId, keySecret).Conversation.Messages;

var request = new ListMessagesRequest
{
    MessagesSource = MessageSource.ConversationSource
};

ListMessagesResponse response = await messagesService.List(request);

Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
}));
