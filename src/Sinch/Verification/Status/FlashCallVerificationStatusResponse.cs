using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Status
{
    public class FlashCallVerificationStatusResponse : VerificationStatusResponseBase, IVerificationStatusResponse
    {
        /// <summary>
        ///     Free text that the client is sending, used to show if the call/SMS was intercepted or not.
        /// </summary>
        public Source? Source { get; set; }

        /// <summary>
        ///     Prices associated with this verification
        /// </summary>
        public Price? Price { get; set; }

        [JsonInclude]
        public override VerificationMethod? Method { get; protected set; } = VerificationMethod.FlashCall;
    }
}
