namespace Sinch.Tests.Features.Sms
{
    public class Utils
    {
        public static ISinchClient SinchClient = new SinchClient(
                new SinchClientConfiguration()
                {
                    SinchUnifiedCredentials = new SinchUnifiedCredentials
                    {
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin"
, KeyId = "keyId",
                        KeySecret = "keySecret"
                    },
                    SinchOptions = new SinchOptions
                    {
                        ApiUrlOverrides = new ApiUrlOverrides()
                        {
                            AuthUrl = "http://localhost:3011",
                            SmsUrl = "http://localhost:3017"
                        }
                    }
                }
            );
    }
}
