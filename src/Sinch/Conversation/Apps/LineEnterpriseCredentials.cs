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

        public LineEnterpriseCredentials(LineJapanEnterpriseCredentials lineJapanEnterpriseCredentials)
        {
            LineJapan = new LineJapan
            {
                Token = lineJapanEnterpriseCredentials.Token,
                Secret = lineJapanEnterpriseCredentials.Secret
            };
            IsDefault = lineJapanEnterpriseCredentials.IsDefault;
        }

        public LineEnterpriseCredentials(LineThailandEnterpriseCredentials lineThailandEnterpriseCredentials)
        {
            LineThailand = new LineThailand()
            {
                Token = lineThailandEnterpriseCredentials.Token,
                Secret = lineThailandEnterpriseCredentials.Secret
            };
            IsDefault = lineThailandEnterpriseCredentials.IsDefault;
        }

        [JsonPropertyName("line_japan")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LineJapan? LineJapan { get; private set; }

        [JsonPropertyName("line_thailand")]
        [JsonInclude]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LineThailand? LineThailand { get; private set; }

        /// <summary>
        ///     When an app contains multiple LINE or LINE Enterprise credentials, one of the credentials needs to be defined as the default. Setting this property to &#x60;true&#x60; marks the corresponding credentials as the default credentials.
        /// </summary>
        [JsonPropertyName("is_default")]
        [JsonInclude]
        public bool IsDefault { get; private set; }

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

    public sealed class LineJapanEnterpriseCredentials : LineCredentials
    {
    }

    public sealed class LineThailandEnterpriseCredentials : LineCredentials
    {
    }

    public class LineCredentialPair
    {
        /// <summary>
        ///     The token for the LINE channel to which you are connecting.
        /// </summary>
        [JsonPropertyName("token")]
        public required string Token { get; set; }

        /// <summary>
        ///     The secret for the LINE channel to which you are connecting.
        /// </summary>
        [JsonPropertyName("secret")]
        public required string Secret { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(LineEnterpriseCredentials)} {{\n");
            sb.Append($"  {nameof(Token)}: ").Append(Consts.HiddenString).Append('\n');
            sb.Append($"  {nameof(Secret)}: ").Append(Consts.HiddenString).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
