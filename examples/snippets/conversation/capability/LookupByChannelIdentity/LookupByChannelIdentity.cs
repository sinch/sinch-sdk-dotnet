using Sinch;
using Sinch.Conversation;
using Sinch.Conversation.Capability;
using Sinch.Conversation.Common;
using Sinch.Core;
using Sinch.Snippets.Shared;

// Your conversation application id
const string applicationId = "MY_APPLICATION_ID";
// Your phone number
const string phoneNumber = "MY_PHONE_NUMBER";

var sinch = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

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
                    Channel = ConversationChannel.Sms,
                    Identity = phoneNumber
                }
            ]
        }
    }
});

Console.WriteLine($"Response: {response.ToPrettyString()}");
