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

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

const string serviceId = "FAX_SERVICE_ID";
const string phoneNumber = "my-virtual-number";
const string emailAddress = "my-email";

Console.WriteLine($"Adding email {emailAddress} to service: {serviceId}");

var request = new EmailRequest
{
    Email = emailAddress,
    PhoneNumbers =
    [
        new()
        {
            Number = phoneNumber,
            Permissions = EmailPermissions.Both
        }
    ]
};

var response = await sinchClient.Fax.Emails.Add(serviceId, request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
