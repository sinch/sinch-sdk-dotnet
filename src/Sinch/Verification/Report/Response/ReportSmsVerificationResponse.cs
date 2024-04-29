using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public class ReportSmsVerificationResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        /// <summary>
        ///     Free text that the client is sending, used to show if the call/SMS was intercepted or not.
        /// </summary>
        public Source? Source { get; set; }

        [JsonInclude]
        public override VerificationMethod? Method { get; protected set; } = VerificationMethod.Sms;

        /// <summary>
        ///     Prices associated with this verification
        /// </summary>
        public PriceBase? Price { get; set; }
    }
}
