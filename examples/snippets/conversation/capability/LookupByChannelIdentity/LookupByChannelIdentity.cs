using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;
using Sinch.Core;
using Sinch.Snippets.Shared;


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
        ConversationRegion = ConversationRegion.Us
    }
});

// The ID of the Conversation App where the recipient channel is configured
const string applicationId = "CONVERSATION_APP_ID";
// The phone number of the recipient to look up the capabilities for
const string recipientPhoneNumber = "RECIPIENT_PHONE_NUMBER";
// The channel to look up the capabilities for
var recipientChannel = ConversationChannel.Sms;

// Lookup capability by a direct channel identity (e.g. a phone number on SMS).
// Use this when you don't have a Conversation API contact_id yet.
var response = await sinch.Conversation.Capabilities.Lookup(new LookupCapabilityRequest
{
    AppId = applicationId,
    Recipient = new Identified
    {
        IdentifiedBy = new IdentifiedBy
        {
            ChannelIdentities =
            [
                new()
                {
                    Channel = recipientChannel,
                    Identity = recipientPhoneNumber
                }
            ]
        }
    }
});

Console.WriteLine($"Response: {response.ToPrettyString()}");
