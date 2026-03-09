using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Transcoding;
using Sinch.Core;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";

// ID of the Conversation application to use for transcoding
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
        ConversationRegion = new ConversationRegion(ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION")
    }
});

var request = new TranscodeRequest
{
    AppId = conversationApplicationId,
    AppMessage = new AppMessage(new LocationMessage
    {
        Title = "Phare d'Eckmühl",
        Coordinates = new Coordinates(47.7981899, -4.3727685)
    }),
    Channels = [ConversationChannel.Sms]
};

Console.WriteLine("Transcoding a location message");

var response = await client.Conversation.Transcoding.Transcode(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
