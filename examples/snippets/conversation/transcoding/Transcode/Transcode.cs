using System.Text.Json;
using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Transcoding;
using Sinch.Core;
using Sinch.Snippets.Shared;

// Your conversation application id
const string applicationId = "APPLICATION_ID";

var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

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
