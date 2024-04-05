namespace Sinch.Verification.Report.Request
{
    public abstract class VerifyReportRequest
    {
        /// <summary>
        ///     The type of verification.
        /// </summary>
        public abstract string Method { get; }
    }
}
