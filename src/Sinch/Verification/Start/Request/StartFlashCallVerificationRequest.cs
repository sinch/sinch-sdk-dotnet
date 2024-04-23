using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public class StartFlashCallVerificationRequest : StartVerificationRequestBase
    {
        /// <summary>
        ///     The type of the verification request. Set to SMS.
        /// </summary>
        [JsonInclude]
        public override VerificationMethodEx Method { get; } = VerificationMethodEx.FlashCall;

        /// <summary>
        ///     An optional object for flashCall verifications.
        ///     It allows you to specify dial time out parameter for flashCall.
        ///     FlashCallOptions object can be specified optionally, and only
        ///     if the verification request was triggered from your backend (no SDK client)
        ///     through an Application signed request.
        /// </summary>
        public FlashCallOptions? FlashCallOptions { get; set; }
    }
}
