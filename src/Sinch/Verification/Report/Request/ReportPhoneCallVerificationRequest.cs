using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Request
{
    public sealed class ReportPhoneCallVerificationRequest : VerifyReportRequest
    {
        public override string Method { get; } = VerificationMethod.Callout.Value;

        /// <summary>
        ///     A configuration object containing settings specific to Phone Call verifications.
        /// </summary>
        public required PhoneCall Callout { get; set; }
    }

    public sealed class PhoneCall
    {
        /// <summary>
        ///     The code which was received by the user submitting the Phone Call verification.
        /// </summary>
        public string? Code { get; set; }
    }
}
