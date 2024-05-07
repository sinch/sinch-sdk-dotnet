using Microsoft.Extensions.Logging;
using Sinch;
using Sinch.SMS;
using Sinch.SMS.Batches.Send;

namespace Examples
{
    public class HandlingExceptions
    {
        public async Task Example()
        {
            // for the sake of example, no real logger is created.
            var logger = LoggerFactory.Create(_ => { }).CreateLogger("example");
            var sinch = new SinchClient(Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
                Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
                Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
            );

            try
            {
                var batch = await sinch.Sms.Batches.Send(new SendTextBatchRequest()
                {
                    Body = "Hello, World!",
                    DeliveryReport = DeliveryReport.None,
                    To = new List<string>()
                    {
                        "+48000000"
                    }
                });
            }
            catch (SinchApiException e)
            {
                logger.LogError("Api Exception. Status: {status}. Detailed message: {message}", e.Status,
                    e.DetailedMessage);
            }
        }
    }
}
