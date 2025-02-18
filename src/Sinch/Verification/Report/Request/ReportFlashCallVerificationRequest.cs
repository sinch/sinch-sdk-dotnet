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
        [JsonPropertyName("flashcall")]
#if NET7_0_OR_GREATER
        public required FlashCall FlashCall { get; set; }
#else
        public FlashCall FlashCall { get; set; } = null!;
#endif
    }

    public sealed class FlashCall
    {
        /// <summary>
        ///     The caller ID of the FlashCall.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Cli { get; set; }
#else
        public string Cli { get; set; } = null!;
#endif
    }
}
