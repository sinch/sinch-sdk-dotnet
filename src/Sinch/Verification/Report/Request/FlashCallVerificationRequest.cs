namespace Sinch.Verification.Report
{
    public class FlashCallVerificationRequest : IVerifyReportRequest
    {
        /// <inheritdoc />
        public override string Method { get; } = "flashCall";

#if NET7_0_OR_GREATER
        public required FlashCall FlashCall { get; set; }
#else
        public FlashCall FlashCall { get; set; }
#endif
    }

    public class FlashCall
    {
#if NET7_0_OR_GREATER
        public required string Cli { get; set; }
#else
    public string Cli {get;set;}
#endif
    }
}
