using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Common;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Send;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var conversationRegion = ConfigurationHelper.GetConversationRegion() ?? "MY_CONVERSATION_REGION";
var sinchVirtualPhoneNumber = ConfigurationHelper.GetPhoneNumber() ?? "SINCH_VIRTUAL_PHONE_NUMBER";

// The ID of the Conversation Application
const string conversationApplicationId = "CONVERSATION_APP_ID";
// The recipient phone number in E.164 format (e.g. +14155552671)
const string recipientPhoneNumber = "RECIPIENT_PHONE_NUMBER";

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

var request = new SendMessageRequest
{
    AppId = conversationApplicationId,
    Recipient = new Identified
    {
        IdentifiedBy = new IdentifiedBy
        {
            ChannelIdentities =
            [
                new ChannelIdentity
                {
                    Channel = ConversationChannel.Sms,
                    Identity = recipientPhoneNumber
                }
            ]
        }
    },
    ChannelProperties = new Dictionary<string, string>
    {
        { "SMS_SENDER", sinchVirtualPhoneNumber }
    },
    Message = new AppMessage(new TextMessage("Hello from the Sinch .NET SDK!"))
};

Console.WriteLine($"Sending SMS to '{recipientPhoneNumber}'...");

var response = await client.Conversation.Messages.Send(request);

Console.WriteLine($"Message sent! Message ID: {response.MessageId}");
