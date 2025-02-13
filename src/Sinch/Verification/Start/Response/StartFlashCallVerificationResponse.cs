namespace Sinch.Verification.Start.Response
{
    public class StartFlashCallVerificationResponse : VerificationStartResponseBase, IStartVerificationResponse
    {
        /// <summary>
        ///     The response contains the cliFilter and interceptionTimeout properties.
        /// </summary>
        public FlashCallDetails? FlashCall { get; set; }
    }

    public class FlashCallDetails
    {
        /// <summary>
        ///     Filter that should be applied for incoming calls to intercept the Flashcall.
        /// </summary>

        public required string CliFilter { get; set; }


        /// <summary>
        ///     Amount of seconds client should wait for the Flashcall.
        /// </summary>

        public required int InterceptionTimeout { get; set; }


        /// <summary>
        ///     The time in seconds allowed for reporting the code after which the verification will expire.
        /// </summary>
        public int? ReportTimeout { get; set; }

        /// <summary>
        ///     Used by the SDKs, this setting makes the handset deny the flashcall after the set time in seconds.
        /// </summary>
        public int? DenyCallAfter { get; set; }
    }
}
