using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If the &#x60;channel&#x60; property is set to &#x60;LINE&#x60; for this entry of the &#x60;channel_credentials&#x60; array, you must include either the &#x60;line_credentials&#x60; object or the &#x60;line_enterprise_credentials&#x60; object in the entry as well.
    /// </summary>
    public sealed class LineCredentials
    {
        /// <summary>
        ///     The token for the LINE channel to which you are connecting.
        /// </summary>
        [JsonPropertyName("token")]
#if NET7_0_OR_GREATER
        public required string Token { get; set; }
#else
        public string Token { get; set; } = null!;
#endif


        /// <summary>
        ///     The secret for the LINE channel to which you are connecting.
        /// </summary>
        [JsonPropertyName("secret")]
#if NET7_0_OR_GREATER
        public required string Secret { get; set; }
#else
        public string Secret { get; set; } = null!;
#endif


        /// <summary>
        ///     When an app contains multiple LINE or LINE Enterprise credentials, one of the credentials needs to be defined as the default. Setting this property to &#x60;true&#x60; marks the corresponding credentials as the default credentials.
        /// </summary>
        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(LineCredentials)} {{\n");
            sb.Append($"  {nameof(Token)}: ").Append(Token).Append('\n');
            sb.Append($"  {nameof(Secret)}: ").Append(Secret).Append('\n');
            sb.Append($"  {nameof(IsDefault)}: ").Append(IsDefault).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
