using System;
using Sinch.SMS;

namespace Sinch.Tests.Sms
{
    public class SmsTestBase : TestBase
    {
        internal readonly ISinchSms Sms;

        protected SmsTestBase()
        {
            Sms = new SMS.SmsClient(ProjectId, new Uri("https://zt.us.sms.api.sinch.com"), default, HttpSnakeCase);
        }
    }
}
