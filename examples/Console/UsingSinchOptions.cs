using Sinch;

namespace Examples;

public class UsingSinchOptions
{
    public void Example()
    {
        var sinch = new SinchClient(new SinchClientConfiguration()
        {
            SinchUnifiedCredentials = new SinchUnifiedCredentials()
            {
                ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            },
            SinchOptions = new SinchOptions()
            {
                HttpClient = new HttpClient()
            }
        });
    }
}
