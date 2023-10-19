namespace Sinch.Verification.Start
{
    public class DataResponse : VerificationResponseBase, IVerificationStartResponse
    {
        /// <summary>
        ///     The response contains the target URI.
        /// </summary>
        public Seamless Seamless { get; set; }
    }

    public class Seamless
    {
        /// <summary>
        ///     The target URI.
        /// </summary>
        public string TargetUri { get; set; }
    }
}
