using Sinch;
using Sinch.SMS;
using Sinch.SMS.Batches.Send;

namespace Examples
{
    public class UseServicePlanIdForSms
    {
        public void Example()
        {
            var sinchClient = new SinchClient(default, default, default,
                options =>
                {
                    options.UseServicePlanIdWithSms(Environment.GetEnvironmentVariable("SINCH_SERVICE_PLAN_ID")!,
                        Environment.GetEnvironmentVariable("SINCH_API_TOKEN")!, SmsServicePlanIdRegion.Ca);
                });
            sinchClient.Sms.Batches.Send(new SendTextBatchRequest()
            {
                Body = "Hello, World!",
                To = new List<string>()
                {
                    "+480000000"
                }
            });
        }
    }
}
