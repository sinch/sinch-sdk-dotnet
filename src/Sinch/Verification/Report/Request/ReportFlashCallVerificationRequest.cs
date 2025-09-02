using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Request
{
    public sealed class ReportFlashCallVerificationRequest : VerifyReportRequest
    {
        /// <inheritdoc />
        public override string Method { get; } = VerificationMethod.FlashCall.Value;

        /// <summary>
        ///     A configuration object containing settings specific to FlashCall verifications.
        /// </summary>
        [JsonPropertyName("flashCall")]
        public required FlashCall FlashCall { get; set; }

    }

    public sealed class FlashCall
    {
        /// <summary>
        ///     The caller ID of the FlashCall.
        /// </summary>

        public required string Cli { get; set; }

    }
}
