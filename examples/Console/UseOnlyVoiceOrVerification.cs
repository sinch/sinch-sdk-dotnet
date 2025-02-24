using Sinch;
using Sinch.Verification;

namespace Examples
{
    public class UseOnlyVoiceOrVerification
    {
        /// <summary>
        ///     If you want to use only voice and/or verification api, you can init sinch client without providing
        ///     common credentials. But be aware that when you try to use other services which depends on the common credentials you will get an exception.
        /// </summary>
        public void Example()
        {
            var sinch = new SinchClient(new SinchClientConfiguration()
            {
                VerificationConfiguration = new SinchVerificationConfiguration()
                {
                    AppKey = "appKey",
                    AppSecret = "appSecret",
                }
            }).Verification;
        }
    }
}
