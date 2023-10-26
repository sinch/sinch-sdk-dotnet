namespace Sinch.Verification.Report.Response
{
    public class FlashCallVerificationReportResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        /// <summary>
        ///     Free text that the client is sending, used to show if the call/SMS was intercepted or not.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        ///     Prices associated with this verification
        /// </summary>
        public Price Price { get; set; }
    }
}
