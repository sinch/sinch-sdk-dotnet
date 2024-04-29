using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public class ReportCalloutVerificationResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        /// <summary>
        ///     Shows whether the call is complete or not.
        /// </summary>
        public bool CallComplete { get; set; }

        /// <summary>
        ///     Prices associated with this verification
        /// </summary>
        public Price? Price { get; set; }

        [JsonInclude]
        public override VerificationMethod? Method { get; protected set; } = VerificationMethod.Callout;
    }
}
