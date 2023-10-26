using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public class VerificationStartRequest
    {
        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint.
        ///     `number` is currently the only supported endpoint type.
        /// </summary>
        public Identity Identity { get; set; }
        
        /// <summary>
        ///     The type of the verification request.
        /// </summary>
        public VerificationMethodEx Method { get; set; }
        
        /// <summary>
        ///     Used to pass your own reference in the request for tracking purposes.
        /// </summary>
        public string Reference { get; set; }
        
        /// <summary>
        ///     Can be used to pass custom data in the request.
        /// </summary>
        public string Custom { get; set; }
        
        /// <summary>
        ///     An optional object for flashCall verifications.
        ///     It allows you to specify dial time out parameter for flashCall.
        ///     FlashCallOptions object can be specified optionally, and only
        ///     if the verification request was triggered from your backend (no SDK client)
        ///     through an Application signed request.
        /// </summary>
        public FlashCallOptions FlashCallOptions { get; set; }
    }

    public class FlashCallOptions
    {
        /// <summary>
        ///    The dial timeout in seconds. 
        /// </summary>
        public int DialTimeout { get; set; }
    }
}
