using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Request
{
    public sealed class ReportCalloutVerificationRequest : VerifyReportRequest
    {
        public override string Method { get; } = VerificationMethod.Callout.Value;

        /// <summary>
        ///     A configuration object containing settings specific to Phone Call verifications.
        /// </summary>
#if NET7_0_OR_GREATER
        public required Callout Callout { get; set; }
#else
        public Callout Callout { get; set; } = null!;
#endif
    }

    public sealed class Callout
    {
        /// <summary>
        ///     The code which was received by the user submitting the Phone Call verification.
        /// </summary>
        public string? Code { get; set; }
    }
}
