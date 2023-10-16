namespace Sinch.Verification.Start
{
    public class SmsResponse : VerificationResponseBase, IVerificationResponse
    {
        public SmsInfo Sms { get; set; }
    }
    
    /// <summary>
    ///     The response contains the template of the SMS to be expected and intercepted.
    /// </summary>
    public class SmsInfo
    {
        public string Template { get; set; }

        public int InterceptionTimeout { get; set; }
    }
}
