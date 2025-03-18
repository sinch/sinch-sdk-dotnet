using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps
{
    public sealed class LineEnterpriseCredentials
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
            sb.Append($"  {nameof(Secret)}: ").Append(Secret).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }

    }
}
