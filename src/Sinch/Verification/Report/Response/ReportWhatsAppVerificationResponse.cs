using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public sealed class ReportWhatsAppVerificationResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        public override VerificationMethod Method { get; protected set; } = VerificationMethod.WhatsApp;
    }
}
