using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public class ReportCalloutVerificationResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        public override VerificationMethod Method { get; protected set; } = VerificationMethod.Callout;

        /// <summary>
        ///     Shows whether the call is complete or not.
        /// </summary>
        public bool? CallComplete { get; set; }
    }
}
