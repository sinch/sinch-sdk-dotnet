/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>

using Sinch;
using Sinch.Core;
using Sinch.Fax;
using Sinch.Fax.Emails;
using Sinch.Snippets.Shared;

var sinchClient = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    },
    FaxConfiguration = new SinchFaxConfiguration
    {
        Region = new FaxRegion(ConfigurationHelper.GetFaxRegion() ?? "MY_FAX_REGION")
    }
});

const string serviceId = "FAX_SERVICE_ID";
const string emailAddress = "my-email";

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

var response = await sinchClient.Fax.Emails.Update(serviceId, emailAddress, request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
