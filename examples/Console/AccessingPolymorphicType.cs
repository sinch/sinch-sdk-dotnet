using Sinch;
using Sinch.Verification.Report.Request;
using Sinch.Verification.Report.Response;

namespace Examples
{
    public class AccessingPolymorphicType
    {
        public static void Example()
        {
            var sinchClient = new SinchClient("KEY_ID", "KEY_SECRET", "PROJECT_ID");
            var response = sinchClient.Verification("APP_KEY", "APP_SECRET").Verification
                .ReportId("id", new SmsVerificationReportRequest()
                {
                    Sms = new SmsVerify()
                    {
                        Code = "123",
                        Cli = "it's a cli"
                    }
                }).Result;
            var id = response switch
            {
                FlashCallVerificationReportResponse flashCallVerificationReportResponse =>
                    flashCallVerificationReportResponse.Id,
                PhoneCallVerificationReportResponse phoneCallVerificationReportResponse =>
                    phoneCallVerificationReportResponse.Id,
                SmsVerificationReportResponse smsVerificationReportResponse => smsVerificationReportResponse.Id,
                _ => throw new ArgumentOutOfRangeException(nameof(response))
            };
            Console.WriteLine(id);
        }
    }
}
