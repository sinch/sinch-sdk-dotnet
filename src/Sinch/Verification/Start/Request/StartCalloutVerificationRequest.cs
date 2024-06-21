using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public class StartCalloutVerificationRequest : StartVerificationRequestBase
    {
        /// <summary>
        ///     The type of the verification request. Set to Phone Call
        /// </summary>
        [JsonInclude]
        public override VerificationMethodEx Method { get; } = VerificationMethodEx.Callout;

        /// <summary>
        ///     Text-To-Speech engine setting. A language-region identifier according to IANA. Only a subset of those identifiers is accepted.
        /// </summary>
        public string? Locale { get; set; }
    }
}
