using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Request
{
    public class ReportFlashCallVerificationRequest : VerifyReportRequest
    {
        /// <inheritdoc />
        public override string Method { get; } = VerificationMethod.FlashCall.Value;

        /// <summary>
        ///     A configuration object containing settings specific to FlashCall verifications.
        /// </summary>
        [JsonPropertyName("flashcall")]

        public required FlashCall FlashCall { get; set; }

    }

    public class FlashCall
    {
        /// <summary>
        ///     The caller ID of the FlashCall.
        /// </summary>

        public required string Cli { get; set; }

    }
}
