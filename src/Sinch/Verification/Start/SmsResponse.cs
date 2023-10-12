namespace Sinch.Verification.Start
{
    public class SmsResponse : IVerificationResponse
    {
        public string Id { get; set; }

        public string Method { get; set; }

        public SmsInfo Sms { get; set; }

        public Links Links { get; set; }
    }

    public class SmsInfo
    {
        public string Template { get; set; }

        public int InterceptionTimeout { get; set; }
    }
}
