using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Start.Response
{
    public class StartWhatsAppVerificationResponse : VerificationStartResponseBase, IStartVerificationResponse
    {
        public StartWhatsAppVerificationResponse()
        {
            Method = VerificationMethodEx.WhatsApp;
        }

        // Hide base property with 'new' keyword if base property is not virtual/abstract/override
        public new VerificationMethodEx Method { get; protected set; }

        /// <summary>
        ///     The response contains the template of the WhatsApp message to be expected and intercepted.
        /// </summary>
        [JsonPropertyName("whatsapp")]
        public WhatsAppInfo? WhatsApp { get; set; }
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
