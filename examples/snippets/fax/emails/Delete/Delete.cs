/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>

using Sinch;
using Sinch.Fax;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";

// The ID of the Fax Service from which you want to delete the email
const string serviceId = "FAX_SERVICE_ID";
const string emailAddress = "MY_EMAIL_ADDRESS";

var client = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = projectId,
        KeyId = keyId,
        KeySecret = keySecret
    },
    FaxConfiguration = new SinchFaxConfiguration
    {
        Region = new FaxRegion(ConfigurationHelper.GetFaxRegion() ?? "MY_FAX_REGION")
    }
});

Console.WriteLine($"Deleting email {emailAddress} from service: {serviceId}");

await client.Fax.Emails.Delete(serviceId, emailAddress);

Console.WriteLine("Delete completed.");
