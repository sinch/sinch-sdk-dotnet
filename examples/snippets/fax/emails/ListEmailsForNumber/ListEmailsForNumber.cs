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

// The Fax Service ID and phone number you want to list emails for
const string serviceId = "FAX_SERVICE_ID";
const string phoneNumber = "my-virtual-number";

Console.WriteLine($"Listing emails for phone number {phoneNumber} in service {serviceId}");

var response = await sinchClient.Fax.Services.ListEmailsForNumber(serviceId, phoneNumber, page: 1, pageSize: 20);

Console.WriteLine($"Response: {response.ToPrettyString()}");
