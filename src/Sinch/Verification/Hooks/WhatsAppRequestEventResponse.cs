using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sinch.Verification.Common;

namespace Sinch.Verification.Hooks
{
    public sealed class WhatsAppRequestEventResponse : RequestEventResponseBase
    {
        [JsonPropertyName("whatsapp")]
        public WhatsApp? WhatsApp { get; set; }
    }

    public sealed class WhatsApp
    {
        /// <summary>
        ///     Accepted values for the type of code to be generated are Numeric, Alpha, and Alphanumeric.
        /// </summary>
        [JsonPropertyName("codeType")]
        public WhatsAppCodeType? CodeType { get; set; }

        /// <summary>
        ///     The SMS verification content language. Set in the verification request.
        /// </summary>
        [JsonPropertyName("acceptLanguage")]
        public List<string>? AcceptLanguage { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new Dictionary<string, JsonElement>();

    }
}
