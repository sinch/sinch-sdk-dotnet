using System.Text.Json;
using Sinch;
using Sinch.Numbers;
using Sinch.Numbers.Available.Rent;
using Sinch.Numbers.VoiceConfigurations;

namespace Examples
{
    public class RentAndConfigureNumbers
    {
        public static async Task Example()
        {
            var sinchClient = new SinchClient(new SinchClientConfiguration()
            {
                SinchUnifiedCredentials = new SinchUnifiedCredentials()
                {
                    ProjectId = "PROJECT_ID",
                    KeyId = "KEY_ID",
                    KeySecret = "KEY_SECRET"
                }
            });
            var response = await sinchClient.Numbers.Rent("+4811111111", new RentActiveNumberRequest()
            {
                SmsConfiguration = new SmsConfiguration
                {
                    ServicePlanId = "your_service_plan_id",
                },
                VoiceConfiguration = new VoiceRtcConfiguration()
                {
                    AppId = "your app id"
                }
            });
            Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
        }
    }
}
