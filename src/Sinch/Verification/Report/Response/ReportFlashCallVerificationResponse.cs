using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Response
{
    public sealed class ReportFlashCallVerificationResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        [JsonInclude]
        public override VerificationMethod Method { get; protected set; } = VerificationMethod.FlashCall;

        /// <summary>
        ///     Shows whether the call is complete or not.
        /// </summary>
        [JsonPropertyName("callComplete")]
        public bool? CallComplete { get; set; }
    }
}
