/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>

using Sinch;
using Sinch.Core;
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
const string emailAddress = "my-email";

Console.WriteLine($"Listing all phone numbers for email {emailAddress}");

await foreach (var number in sinchClient.Fax.Emails.ListNumbersAuto(serviceId, emailAddress, pageSize: 20))
{
    Console.WriteLine(number.ToPrettyString());
}
