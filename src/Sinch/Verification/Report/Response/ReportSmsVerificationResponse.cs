using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public class ReportSmsVerificationResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        public override VerificationMethod Method { get; protected set; } = VerificationMethod.Sms;
    }
}
