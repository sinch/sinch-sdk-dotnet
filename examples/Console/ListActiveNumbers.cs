using Sinch;
using Sinch.Numbers;
using Sinch.Numbers.Active.List;

namespace Examples;

public class ListActiveNumbers
{
    public async Task Example()
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
        ListActiveNumbersResponse response = await sinch.Numbers.List(new ListActiveNumbersRequest
        {
            RegionCode = "US",
            Type = Types.Mobile
        });
    }
}
