using System.Collections.Generic;
using System.Text.Json;
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
        ///     The response contains the template of the WhatsApp message to be expected and intercepted.
        /// </summary>
        [JsonPropertyName("whatsappOptions")]
        public WhatsAppInfo? WhatsAppInfo { get; set; }

    }
    /// <summary>
    ///     The response contains the template of the WhatsApp message to be expected and intercepted.
    /// </summary>
    public sealed class WhatsAppInfo
    {
        /// <summary>
        ///     Accepted values for the type of code to be generated are Numeric, Alpha, and Alphanumeric.
        /// </summary>
        [JsonPropertyName("codeType")]
        public WhatsAppCodeType? CodeType { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new Dictionary<string, JsonElement>();

    }

}
