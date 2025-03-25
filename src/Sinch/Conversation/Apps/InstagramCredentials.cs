using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     If you are including the INSTAGRAM channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class InstagramCredentials
    {
        /// <summary>
        ///     The static token.
        /// </summary>
        [JsonPropertyName("token")]
#if NET7_0_OR_GREATER
        public required string Token { get; set; }
#else
        public string Token { get; set; } = null!;
#endif


        /// <summary>
        ///     Required if using the Sinch Facebook App.
        /// </summary>
        [JsonPropertyName("business_account_id")]
        public string? BusinessAccountId { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(InstagramCredentials)} {{\n");
            sb.Append($"  {nameof(Token)}: ").Append(Consts.HiddenString).Append('\n');
            sb.Append($"  {nameof(BusinessAccountId)}: ").Append(Consts.HiddenString).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
