/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet demonstrates deleting the content of a fax from the server.
/// Note: This only deletes the content, not the fax record itself.
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Fax.Faxes;
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

Console.WriteLine("Deleting fax content on the server");

const string faxId = "FAX_ID";

await sinchClient.Fax.Faxes.DeleteContent(faxId);

Console.WriteLine("Fax content deleted successfully");
