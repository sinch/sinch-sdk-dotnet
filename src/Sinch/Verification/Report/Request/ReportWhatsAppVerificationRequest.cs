using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Request
{
    public sealed class ReportWhatsAppVerificationRequest : VerifyReportRequest
    {
        [JsonPropertyName("method")]
        public override string Method { get; } = VerificationMethod.WhatsApp.Value;

        /// <summary>
        ///     A configuration object containing settings specific to WhatsApp verifications.
        /// </summary>
        [JsonPropertyName("whatsapp")]
        public WhatsApp? WhatsApp { get; set; }
    }

    public sealed class WhatsApp
    {
        /// <summary>
        ///     The code which was received by the user submitting the WhatsApp verification.
        /// </summary>
        [JsonPropertyName("code")]
        public required string Code { get; set; }
    }
}
