using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public class ReportCalloutVerificationResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        public override VerificationMethod Method { get; protected set; } = VerificationMethod.Callout;
    }
}
