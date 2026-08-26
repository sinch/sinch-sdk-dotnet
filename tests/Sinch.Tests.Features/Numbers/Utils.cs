using Sinch.Numbers;

namespace Sinch.Tests.Features.Numbers
{
    public class Utils
    {
        public static ISinchNumbers SinchNumbersClient()
        {
            return new SinchClient("tinyfrog-jump-high-over-lilypadbasin", "keyId", "keySecret", options =>
            {
                options.ApiUrlOverrides = new ApiUrlOverrides()
                {
                    AuthUrl = Helpers.MOCKSERVER_AUTH_URL,
                    NumbersUrl = Helpers.MOCKSERVER_NUMBERS_URL
                };
            }).Numbers;
        }
    }
}
