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
                    AuthUrl = "http://localhost:3011",
                    NumbersUrl = "http://localhost:3013"
                };
            }).Numbers;
        }
    }
}
