using Sinch;
using Sinch.Verification.Start.Request;
using Sinch.Verification.Start.Response;

namespace Examples
{
    public class AccessingPolymorphicType
    {
        public static void Example()
        {
            var sinchClient = new SinchClient("KEY_ID", "KEY_SECRET", "PROJECT_ID");
            var verificationStart = sinchClient.Verification("APP_KEY", "APP_SECRET").Verification
                .Start(new VerificationStartRequest()).Result;
            var verificationStartId = verificationStart switch
            {
                DataVerificationStartResponse dataVerificationStartResponse => dataVerificationStartResponse.Id,
                FlashCallVerificationStartResponse flashCallVerificationStartResponse =>
                    flashCallVerificationStartResponse.Id,
                PhoneCallVerificationStartResponse phoneCallVerificationStartResponse =>
                    phoneCallVerificationStartResponse.Id,
                SmsVerificationStartResponse smsVerificationStartResponse => smsVerificationStartResponse.Id,
                _ => throw new ArgumentOutOfRangeException(nameof(verificationStart))
            };
            Console.WriteLine(verificationStartId);
        }
    }
}
