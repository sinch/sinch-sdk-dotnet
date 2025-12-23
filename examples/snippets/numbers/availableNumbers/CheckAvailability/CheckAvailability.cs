using Sinch;
using Sinch.Core;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID") ?? "MY_PROJECT_ID",
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID") ?? "MY_KEY_ID",
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET") ?? "MY_KEY_SECRET"
    }
});

var phoneNumber = "A_PHONE_NUMBER_TO_CHECK";

Console.WriteLine($"CheckAvailability for: {phoneNumber}");

var response = await sinchClient.Numbers.CheckAvailability(phoneNumber);

Console.WriteLine($"Response: {response.ToJson()}");
