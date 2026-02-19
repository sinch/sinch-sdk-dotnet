/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet demonstrates retrieving a specific fax by ID.
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
using Sinch;
using Sinch.Core;
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

Console.WriteLine("Getting a fax by ID");

const string faxId = "FAX_ID";

var fax = await sinchClient.Fax.Faxes.Get(faxId);

Console.WriteLine($"Response: {fax.ToPrettyString()}");
