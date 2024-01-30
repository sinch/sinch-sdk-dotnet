using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public sealed class StartSmsVerificationRequest : StartVerificationRequestBase
    {
        /// <summary>
        ///     The type of the verification request. Set to SMS.
        /// </summary>
        [JsonInclude]
        public override VerificationMethodEx Method { get; } = VerificationMethodEx.Sms;
    }
}
