using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Transcoding;
using Sinch.Core;
using Sinch.Snippets.Shared;

var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

// The ID of the Conversation App where the recipient channel is configured
const string applicationId = "CONVERSATION_APP_ID";

var request = new TranscodeRequest
{
    AppId = applicationId,
    AppMessage = new AppMessage(new LocationMessage
    {
        Title = "Phare d'Eckmühl",
        Coordinates = new Coordinates(47.7981899, -4.3727685)
    }),
    Channels = [ConversationChannel.Sms]
};

Console.WriteLine("Transcoding a location message");

var response = await sinch.Conversation.Transcoding.Transcode(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
