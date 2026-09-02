using Sinch.Fax;

namespace Sinch.Tests.Features.Fax;

public class Utils
{
    public static ISinchFax SinchFaxClient()
    {
        return new SinchClient(
            new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials
                {
                    ProjectId = "tinyfrog-jump-high-over-lilypadbasin",
                    KeyId = "keyId",
                    KeySecret = "keySecret"
                },
                SinchOptions = new SinchOptions
                {
                    ApiUrlOverrides = new ApiUrlOverrides()
                    {
                        AuthUrl = Helpers.MOCKSERVER_AUTH_URL,
                        FaxUrl = Helpers.MOCKSERVER_FAX_URL
                    }
                }
            }
        ).Fax;
    }
}
