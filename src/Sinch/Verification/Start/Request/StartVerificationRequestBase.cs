using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public abstract class StartVerificationRequestBase
    {
        [JsonInclude]
        public abstract VerificationMethodEx Method { get; }

        /// <summary>
        ///     Specifies the type of endpoint that will be verified and the particular endpoint.
        ///     `number` is currently the only supported endpoint type.
        /// </summary>
        public Identity? Identity { get; set; }

        /// <summary>
        ///     Used to pass your own reference in the request for tracking purposes.
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        ///     Can be used to pass custom data in the request.
        /// </summary>
        public string? Custom { get; set; }
    }
}
