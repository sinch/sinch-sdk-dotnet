using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Status
{
    public sealed class WhatsAppVerificationStatusResponse : VerificationStatusResponseBase, IVerificationStatusResponse
    {
        /// <summary>
        ///     Free text that the client is sending, used to show if the call/WhatsApp was intercepted or not.
        /// </summary>
        [JsonPropertyName("source")]
        public Source? Source { get; set; }

        [JsonInclude]
        [JsonPropertyName("method")]
        public override VerificationMethod? Method { get; protected set; } = VerificationMethod.WhatsApp;

        /// <summary>
        ///     Prices associated with this verification
        /// </summary>
        [JsonPropertyName("price")]
        public PriceBase? Price { get; set; }
    }
}
