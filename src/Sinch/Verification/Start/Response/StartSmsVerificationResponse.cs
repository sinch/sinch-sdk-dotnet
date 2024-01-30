namespace Sinch.Verification.Start.Response
{
    public class StartSmsVerificationResponse : VerificationStartResponseBase, IVerificationStartResponse
    {
        /// <summary>
        ///     The response contains the template of the SMS to be expected and intercepted.
        /// </summary>
        public SmsInfo Sms { get; set; }
    }
    
    /// <summary>
    ///     The response contains the template of the SMS to be expected and intercepted.
    /// </summary>
    public class SmsInfo
    {
        /// <summary>
        ///     The expected template for the SMS response.
        /// </summary>
        public string Template { get; set; }
        
        /// <summary>
        ///     The amount of time in seconds that the client should wait for the SMS.
        /// </summary>
        public int? InterceptionTimeout { get; set; }
    }
}
