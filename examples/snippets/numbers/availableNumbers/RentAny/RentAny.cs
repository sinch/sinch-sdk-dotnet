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
using Sinch.Numbers.Available.RentAny;

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

// ISO 3166-1 alpha-2 country code of the phone number. e.g. "US", "GB", "SE"...
// See https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2 for details
var regionCode = "MY_REGION_CODE";

var numberType = Types.Local;

Console.WriteLine("RentAny number");

var request = new RentAnyNumberRequest
{
    RegionCode = regionCode,
    Type = numberType,
    SmsConfiguration = new SmsConfiguration
    {
        ServicePlanId = servicePlanId
    }
};

var response = await sinchClient.Numbers.RentAny(request);

Console.WriteLine($"Response: {response.ToPrettyString()}");
