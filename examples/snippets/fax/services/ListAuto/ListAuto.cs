/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>
/// 
using Sinch;
using Sinch.Core;
using Sinch.Snippets.Shared;

var sinchClient = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
    {
        ProjectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID",
        KeyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID",
        KeySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET"
    }
});

Console.WriteLine("Listing all fax services");

// ListAuto handles pagination automatically, but you can control the page size
await foreach (var service in sinchClient.Fax.Services.ListAuto(page: 1, pageSize: 20))
{
    Console.WriteLine(service.ToPrettyString());
}
