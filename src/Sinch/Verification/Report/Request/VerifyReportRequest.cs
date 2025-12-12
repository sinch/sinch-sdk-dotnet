using System.Text.Json.Serialization;

namespace Sinch.Verification.Report.Request
{
    public abstract class VerifyReportRequest
    {
        /// <summary>
        ///     The type of verification.
        /// </summary>
        [JsonPropertyName("method")]
        public abstract string Method { get; }
    }
}
