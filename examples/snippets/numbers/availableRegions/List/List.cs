using Sinch;
using Sinch.Numbers;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID") ?? "MY_PROJECT_ID",
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID") ?? "MY_KEY_ID",
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET") ?? "MY_KEY_SECRET"
    }
});

Console.WriteLine("List available regions");

var response = await sinchClient.Numbers.Regions.List(new[] { Types.Mobile, Types.Local, Types.TollFree });

Console.WriteLine("Available regions:");

foreach (var region in response)
{
    Console.WriteLine($"- {region.RegionCode}: {region.RegionName} ({string.Join(", ", region.Types)})");
}
