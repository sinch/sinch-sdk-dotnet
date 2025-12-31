/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>

using Sinch;
using Sinch.Core;
using Sinch.Numbers.Active.Update;
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

var phoneNumber = ConfigurationHelper.GetPhoneNumber() ?? "MY_SINCH_PHONE_NUMBER";
const string displayName = "Updated with Sinch C# SDK";

Console.WriteLine($"Updating number: {phoneNumber}");

var response = await sinchClient.Numbers.Update(phoneNumber, new UpdateActiveNumberRequest
{
    DisplayName = displayName
});

Console.WriteLine($"Response: {response.ToPrettyString()}");
