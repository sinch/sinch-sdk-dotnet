/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>

using Sinch;
using Sinch.Core;
using Sinch.Fax.Emails;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";

// The ID of the Fax Service for which you want to update the phone numbers for a specific email address
const string serviceId = "FAX_SERVICE_ID";
const string emailAddress = "MY_EMAIL_ADDRESS";

var client = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    }
});

Console.WriteLine($"Updating phone numbers for email {emailAddress}");

var request = new UpdateEmailRequest
{
    PhoneNumbers = new List<PhoneNumber>
    {
        new()
        {
            Number = "my-phone-number-1",
            Permissions = PhoneNumberPermission.Send
        },
        new()
        {
            Number = "my-phone-number-2",
            Permissions = PhoneNumberPermission.Receive
        }
    }
};

var response = await client.Fax.Emails.Update(serviceId, emailAddress, request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
