namespace Sinch.Verification.Report
{
    public abstract class IVerifyReportRequest
    {
        /// <summary>
        ///     The type of verification.
        /// </summary>
        public abstract string Method { get; }
    }
}
