namespace Sinch.Verification.Report
{
    public class SmsVerificationRequest : IVerifyRequest
    {
        public override string Method { get; } = "sms";

        public SmsVerify Sms { get; set; }
    }

    public class SmsVerify
    {
#if NET7_0_OR_GREATER
        public required string Code { get; set; }
#else
         public  string Code { get; set; }
#endif
        public string Cli { get; set; }
    }
}
