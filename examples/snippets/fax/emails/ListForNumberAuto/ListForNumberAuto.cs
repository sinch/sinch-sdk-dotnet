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

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
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
const string phoneNumber = "my-virtual-number";

Console.WriteLine($"Listing all emails for phone number {phoneNumber}");

await foreach (var email in sinchClient.Fax.Emails.ListForNumberAuto(serviceId, phoneNumber))
{
    Console.WriteLine(email);
}
