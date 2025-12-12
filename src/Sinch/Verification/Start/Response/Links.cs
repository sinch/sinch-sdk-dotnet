using System.Text.Json.Serialization;

namespace Sinch.Verification.Start.Response
{
    public sealed class Links
    {
        /// <summary>
        ///     The related action that can be performed on the initiated Verification.
        /// </summary>
        [JsonPropertyName("rel")]
        public string? Rel { get; set; }

        /// <summary>
        ///     The complete URL to perform the specified action,
        ///     localized to the DataCenter which handled the original Verification request.
        /// </summary>
        [JsonPropertyName("href")]
        public string? Href { get; set; }

        /// <summary>
        ///     The HTTP method to use when performing the action using the linked localized URL.
        /// </summary>
        [JsonPropertyName("method")]
        public string? Method { get; set; }
    }
}
