using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Request
{
    public sealed class StartWhatsAppVerificationRequest : StartVerificationRequestBase
    {
        /// <summary>
        ///     The type of the verification request. Set to WhatsApp.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("method")]
        public override VerificationMethodEx Method { get; } = VerificationMethodEx.WhatsApp;

        /// <summary>
        ///     An optional object for WhatsApp verifications.
        /// </summary>
        [JsonPropertyName("whatsappOptions")]
        public WhatsAppOptions? WhatsAppOptions { get; set; }
    }

}
