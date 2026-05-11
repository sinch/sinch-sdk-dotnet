using Sinch.SMS;

namespace Sinch.Tests.Sms
{
    public class SmsTestBase : TestBase
    {
        internal readonly ISinchSms Sms;

        protected SmsTestBase()
        {
            Sms = new SmsClient(
                new SinchSmsConfiguration { Region = SmsRegion.Us },
                ProjectId,
                null,
                default,
                HttpSnakeCase,
                () => HttpClient);
        }
    }
}
