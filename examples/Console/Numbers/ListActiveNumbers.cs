using Sinch;
using Sinch.Numbers;
using Sinch.Numbers.Active.List;

namespace Examples.Numbers;

public class ListActiveNumbers
{
    public async Task Run()
    {
        var sinch = new SinchClient(new SinchClientConfiguration()
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials()
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            }
        });

        var numbers = await sinch.Numbers.List(new ListActiveNumbersRequest
        {
            RegionCode = "GB",
            Type = Types.Mobile
        });

        Console.WriteLine("Active Numbers in United Kingdom:");

        foreach (var number in numbers.ActiveNumbers)
        {
            Console.WriteLine($"Active Number: {number.PhoneNumber}, Type: {number.Type}, Region: {number.RegionCode}");
        }
    }
}
