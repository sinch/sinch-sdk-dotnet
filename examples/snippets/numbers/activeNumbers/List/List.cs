using Sinch;
using Sinch.Core;
using Sinch.Numbers;
using Sinch.Numbers.Active.List;

var sinchClient = new SinchClient(new SinchClientConfiguration()
{
    SinchUnifiedCredentials = new SinchUnifiedCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")??"MY_PROJECT_ID",
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")??"MY_KEY_ID",
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")??"MY_KEY_SECRET"
    }
});

var request = new ListActiveNumbersRequest
{
    RegionCode = "US",
    Type = Types.Local
};

Console.WriteLine("Listing active numbers");

var response = await sinchClient.Numbers.List(request);

Console.WriteLine($"Response: {response.ToJson()}");
