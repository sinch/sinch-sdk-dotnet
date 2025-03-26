using Sinch;
using Sinch.Verification;
using Sinch.Verification.Common;
using Sinch.Verification.Start.Request;
using Sinch.Voice;

namespace Examples
{
    public class UseOnlyVoiceOrVerification
    {
        /// <summary>
        ///     If you want to use only voice and/or verification api, you can init sinch client without providing
        ///     unified credentials. But be aware that when you try to use other services which depends on the common credentials you will get an exception.
        /// </summary>
        public async Task Example()
        {
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                VerificationConfiguration = new SinchVerificationConfiguration()
                {
                    AppKey = "verificationAppKey",
                    AppSecret = "verificationAppSecret",
                },
                VoiceConfiguration = new SinchVoiceConfiguration()
                {
                    AppKey = "voiceAppKey",
                    AppSecret = "voiceAppSecret",
                }
            });
            // example of voice use
            var voiceClient = sinch.Voice;
            var call = await voiceClient.Calls.Get("1");
            // do smth with call response

            // example of verification use
            var verificationClient = sinch.Verification;
            var verificationStart = await verificationClient.Verification.StartSms(new StartSmsVerificationRequest()
            {
                Identity = Identity.Number("+481123123123")
            });
            // do smth with verification start response 
        }
    }
}
