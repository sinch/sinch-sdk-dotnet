namespace Sinch.Tests.Features.Sms
{
    public class Utils
    {
        public static ISinchClient SinchClient = new SinchClient("tinyfrog-jump-high-over-lilypadbasin", "keyId", "keySecret",
            options =>
            {
                options.ApiUrlOverrides = new ApiUrlOverrides()
                {
                    AuthUrl = "http://localhost:3011",
                    SmsUrl = "http://localhost:3017"
                };
            });
    }
}
