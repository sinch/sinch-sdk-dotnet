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
using Sinch.Fax;
using Sinch.Snippets.Shared;

var projectId = ConfigurationHelper.GetProjectId() ?? "MY_PROJECT_ID";
var keyId = ConfigurationHelper.GetKeyId() ?? "MY_KEY_ID";
var keySecret = ConfigurationHelper.GetKeySecret() ?? "MY_KEY_SECRET";
var faxRegion = ConfigurationHelper.GetFaxRegion() ?? "MY_FAX_REGION";

// The Fax Service ID for which you want to list numbers for
const string serviceId = "FAX_SERVICE_ID";

var client = new SinchClient(new SinchClientConfiguration
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials
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

Console.WriteLine($"Listing all numbers for fax service: {serviceId}");

// ListNumbersAuto handles pagination automatically, but you can control the page size
await foreach (var number in client.Fax.Services.ListNumbersAuto(serviceId))
{
    Console.WriteLine(number.ToPrettyString());
}
