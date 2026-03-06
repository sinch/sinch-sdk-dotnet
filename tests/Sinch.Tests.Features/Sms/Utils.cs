using Sinch.SMS;

namespace Sinch.Tests.Features.Sms
{
    public class Utils
    {
        public static readonly ISinchClient SinchClient = new SinchClient(
                new SinchClientConfiguration
                {
                    SinchUnifiedCredentials = new SinchUnifiedCredentials
                    {
                        ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                        KeyId = "keyId",
                        KeySecret = "keySecret"
                    },
                    SmsConfiguration = new SinchSmsConfiguration
                    {
                        Region = SmsRegion.Us
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

        public static readonly ISinchClient SinchClientServicePlanId = new SinchClient(
                new SinchClientConfiguration
                {
                    SmsConfiguration = SinchSmsConfiguration.WithServicePlanId(
                        "CappyPremiumPlan",
                        "HappyCappyToken",
                        urlOverride: "http://localhost:3017")
                }
            );
    }
}
