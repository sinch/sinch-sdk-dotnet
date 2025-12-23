/// <summary>
/// Sinch .NET SDK Snippet
/// 
/// This snippet is available at https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets
/// 
/// See https://github.com/sinch/sinch-sdk-dotnet/blob/main/examples/snippets/README.md for details
/// </summary>

using Sinch;
using Sinch.Core;
using Sinch.Numbers;
using Sinch.Numbers.Available.Rent;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID") ?? "MY_PROJECT_ID",
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID") ?? "MY_KEY_ID",
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET") ?? "MY_KEY_SECRET"
    }
});

var servicePlanId = Environment.GetEnvironmentVariable("SINCH_SERVICE_PLAN_ID") ?? "MY_SERVICE_PLAN_ID";

// Available numbers list can be retrieved by using SearchForAvailableNumbers() function, see
// the SearchForAvailableNumbers snippet or
// https://developers.sinch.com/docs/numbers/getting-started/dotnet-sdk/searchavailable
var phoneNumberToBeRented = "AVAILABLE_PHONE_NUMBER_TO_BE_RENTED";

Console.WriteLine($"Rent for: {phoneNumberToBeRented}");

var request = new RentActiveNumberRequest
{
    SmsConfiguration = new SmsConfiguration
    {
        ServicePlanId = servicePlanId
    }
};

var response = await sinchClient.Numbers.Rent(phoneNumberToBeRented, request);

Console.WriteLine($"Response: {response.ToJson()}");
