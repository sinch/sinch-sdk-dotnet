using Sinch.Verification.Common;

namespace Sinch.Verification.Report.Request
{
    public class FlashCallVerificationReportRequest : VerifyReportRequest
    {
        /// <inheritdoc />
        public override string Method { get; } = VerificationMethod.FlashCall.Value;

        /// <summary>
        ///     A configuration object containing settings specific to FlashCall verifications.
        /// </summary>
#if NET7_0_OR_GREATER
        public required FlashCall FlashCall { get; set; }
#else
        public FlashCall FlashCall { get; set; }
#endif
    }

    public class FlashCall
    {
            
        /// <summary>
        ///     The caller ID of the FlashCall.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string Cli { get; set; }
#else
        public string Cli {get;set;}
#endif
    }
}
