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

// The service ID to which you want to add the email
const string serviceId = "FAX_SERVICE_ID";
const string phoneNumber = "MY_PHONE_NUMBER";
const string emailAddress = "MY_EMAIL_ADDRESS";

var client = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    }
});

Console.WriteLine($"Adding email {emailAddress} to service: {serviceId}");

var request = new EmailRequest
{
    Email = emailAddress,
    PhoneNumbers =
    [
        new()
        {
            Number = phoneNumber,
            Permissions = PhoneNumberPermission.Both
        }
    ]
};

var response = await client.Fax.Emails.Add(serviceId, request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
