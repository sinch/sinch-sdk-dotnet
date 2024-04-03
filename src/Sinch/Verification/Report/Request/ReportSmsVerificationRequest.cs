using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Request
{
    public class ReportSmsVerificationRequest : VerifyReportRequest
    {
        public override string Method { get; } = VerificationMethod.Sms.Value;

        /// <summary>
        ///     A configuration object containing settings specific to SMS verifications.
        /// </summary>
        public SmsVerify Sms { get; set; }
    }

    public class SmsVerify
    {
        /// <summary>
        ///     The code which was received by the user submitting the SMS verification.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Code { get; set; }
#else
        public string Code { get; set; }
#endif
        /// <summary>
        ///     The sender ID of the SMS.
        /// </summary>
        public string Cli { get; set; }
    }
}
