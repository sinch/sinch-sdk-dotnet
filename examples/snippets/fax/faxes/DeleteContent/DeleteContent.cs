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
var faxRegion = ConfigurationHelper.GetFaxRegion() ?? "MY_FAX_REGION";

// The ID of the Fax you want to delete content for
const string faxId = "FAX_ID";

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
        Region = new FaxRegion(faxRegion)
    }
});

Console.WriteLine("Deleting fax content on the server");

await client.Fax.Faxes.DeleteContent(faxId);

Console.WriteLine("Fax content deleted successfully");
