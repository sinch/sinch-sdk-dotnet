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
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";

// The ID of the Fax Service for which you want to list the emails for a specific phone number
const string serviceId = "FAX_SERVICE_ID";
const string phoneNumber = "MY_PHONE_NUMBER";

var client = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    }
});

Console.WriteLine($"Listing emails for phone number {phoneNumber} in service {serviceId}");

var response = await client.Fax.Emails.ListForNumber(serviceId, phoneNumber);

Console.WriteLine($"Response: {response.ToPrettyString()}");
