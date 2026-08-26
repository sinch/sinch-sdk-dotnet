namespace Sinch.Tests.Features.Sms
{
    public class Utils
    {
        public static ISinchClient SinchClient = new SinchClient("tinyfrog-jump-high-over-lilypadbasin", "keyId", "keySecret",
            options =>
            {
                options.ApiUrlOverrides = new ApiUrlOverrides()
                {
                    AuthUrl = Helpers.MOCKSERVER_AUTH_URL,
                    SmsUrl = Helpers.MOCKSERVER_SMS_URL
                };
            });

        public static ISinchClient SinchClientServicePlanId = new SinchClient(null, null, null,
                    options =>
                    {
                        options.ApiUrlOverrides = new ApiUrlOverrides()
                        {
                            AuthUrl = Helpers.MOCKSERVER_AUTH_URL,
                            SmsUrl = Helpers.MOCKSERVER_SMS_URL
                        };
                        options.UseServicePlanIdWithSms("CappyPremiumPlan", "HappyCappyToken");
                    });

    }
}
