using System;
using System.Text;
using System.Text.Json.Serialization;
using Sinch.Conversation.Apps.Credentials;

namespace Sinch.Conversation.Apps
{
    public sealed class LineEnterpriseCredentials
    {
        [Obsolete("Required for System.Text.Json", error: true)]
        public LineEnterpriseCredentials()
        {
        }

        public LineEnterpriseCredentials(LineJapan lineJapan)
        {
            LineJapan = lineJapan;
        }

        public LineEnterpriseCredentials(LineThailand lineThailand)
        {
            LineThailand = lineThailand;
        }

        [JsonPropertyName("line_japan")]
        [JsonInclude]
        public LineJapan? LineJapan { get; private set; }

        [JsonPropertyName("line_thailand")]
        [JsonInclude]
        public LineThailand? LineThailand { get; private set; }

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
            sb.Append($"class {nameof(LineEnterpriseCredentials)} {{\n");
            sb.Append($"  {nameof(LineThailand)}: ").Append(LineThailand).Append('\n');
            sb.Append($"  {nameof(LineJapan)}: ").Append(LineJapan).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    public sealed class LineJapan : LineCredentialPair
    {
    }

    public sealed class LineThailand : LineCredentialPair
    {
    }

    public class LineCredentialPair
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
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(LineEnterpriseCredentials)} {{\n");
            sb.Append($"  {nameof(Token)}: ").Append(Token).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
