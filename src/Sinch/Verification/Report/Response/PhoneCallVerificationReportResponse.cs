namespace Sinch.Verification.Report.Response
{
    public class PhoneCallVerificationReportResponse : VerificationReportResponseBase, IVerificationReportResponse
    {
        /// <summary>
        ///     Shows whether the call is complete or not.
        /// </summary>
        public bool CallComplete { get; set; }

        /// <summary>
        ///     Prices associated with this verification
        /// </summary>
        public Price Price { get; set; }
    }
}
