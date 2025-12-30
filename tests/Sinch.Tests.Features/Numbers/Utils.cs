using Sinch.Numbers;

namespace Sinch.Tests.Features.Numbers
{
    public class Utils
    {
        public static ISinchNumbers SinchNumbersClient()
        {

            return new SinchClient(
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
                               NumbersUrl = "http://localhost:3013"
                           }
                       }
                   }
               ).Numbers;
        }
    }
}
