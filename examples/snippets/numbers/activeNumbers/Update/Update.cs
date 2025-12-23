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

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID") ?? "MY_PROJECT_ID",
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID") ?? "MY_KEY_ID",
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET") ?? "MY_KEY_SECRET"
    }
});

var phoneNumber = Environment.GetEnvironmentVariable("SINCH_PHONE_NUMBER") ?? "MY_SINCH_PHONE_NUMBER";
const string displayName = "Updated with Sinch C# SDK";

Console.WriteLine($"Updating number: {phoneNumber}");

var response = await sinchClient.Numbers.Update(phoneNumber, new UpdateActiveNumberRequest
{
    DisplayName = displayName
});

Console.WriteLine($"Response: {response.ToJson()}");
