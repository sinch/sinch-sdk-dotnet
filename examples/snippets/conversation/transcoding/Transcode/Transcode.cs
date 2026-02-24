using System.Text.Json;
using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Transcoding;
using Sinch.Snippets.Shared;

const string appId = "MY_APP_ID";
const string conversationRegion = "MY_CONVERSATION_REGION";

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
        ConversationRegion = new ConversationRegion(conversationRegion)
    }
});

var request = new TranscodeRequest
{
    AppId = appId,
    AppMessage = new AppMessage(new LocationMessage
    {
        Title = "Phare d'Eckmühl",
        Label = "Pointe de Penmarch",
        Coordinates = new Coordinates(47.7981899, -4.3727685)
    }),
    Channels = new List<ConversationChannel>
    {
        ConversationChannel.Sms
    }
};

Console.WriteLine("Transcoding a location message");

var response = await sinch.Conversation.Transcoding.Transcode(request);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
