using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public class StartDataVerificationRequest : StartVerificationRequestBase
    {
        /// <summary>
        ///     The type of the verification request. Set to Seamless
        /// </summary>
        [JsonInclude]
        public override VerificationMethodEx Method { get; } = VerificationMethodEx.Seamless;
    }
}
