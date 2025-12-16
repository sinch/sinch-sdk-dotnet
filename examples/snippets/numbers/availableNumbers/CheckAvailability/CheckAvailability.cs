using System.Text.Json;
using Sinch;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID") ?? "MY_PROJECT_ID",
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID") ?? "MY_KEY_ID",
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET") ?? "MY_KEY_SECRET"
    }
});

var phoneNumber = "+447418629954";

Console.WriteLine($"CheckAvailability for: {phoneNumber}");

var response = await sinchClient.Numbers.CheckAvailability(phoneNumber);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");
