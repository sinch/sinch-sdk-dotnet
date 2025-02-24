using Sinch;

namespace Examples
{
    public class UseOnlyVoiceOrVerification
    {
        /// <summary>
        ///     If you want to use only voice and/or verification api, you can init sinch client without providing
        ///     necessary credentials. But be aware that when you try to use other services which depends on the common credentials you will get an exception.
        /// </summary>
        public void Example()
        {
            var sinchVoiceClient = new SinchClient(new SinchClientConfiguration()).Voice("appKey", "appSecret");
            var sinchVerificationClient =
                new SinchClient(new SinchClientConfiguration()).Verification("appKey", "appSecret");
        }
    }
}
