using System.Text.Json;
using Sinch;
using Sinch.Numbers;
using Sinch.Numbers.Available.List;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID") ?? "MY_PROJECT_ID",
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID") ?? "MY_KEY_ID",
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET") ?? "MY_KEY_SECRET"
    }
});

// ISO 3166-1 alpha-2 country code of the phone number. e.g. "US", "GB", "SE"...
// See https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2 for details
var regionCode = "MY_REGION_CODE";
var type = Types.Local;

var request = new ListAvailableNumbersRequest
{
    RegionCode = regionCode,
    Type = type
};

Console.WriteLine("Looking for available numbers");

var response = await sinchClient.Numbers.SearchForAvailableNumbers(request);

var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions()
{
    WriteIndented = true
});

Console.WriteLine($"Response: {jsonResponse}");

