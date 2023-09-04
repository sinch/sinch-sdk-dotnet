using System.Text;

namespace Sinch.Conversation.Apps.Credentials
{
    /// <summary>
    ///     If you are including the MMS channel in the &#x60;channel_identifier&#x60; property, you must include this object.
    /// </summary>
    public sealed class MmsCredentials
    {
        /// <summary>
        ///     MMS Account ID.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string AccountId { get; set; }
#else
        public string AccountId { get; set; }
#endif


        /// <summary>
        ///     MMS API Key.
        /// </summary>
#if NET7_0_OR_GREATER
        public required string ApiKey { get; set; }
#else
        public string ApiKey { get; set; }
#endif


        /// <summary>
        ///     Gets or Sets BasicAuth
        /// </summary>
        public BasicAuthCredential BasicAuth { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MMSCredentials {\n");
            sb.Append("  AccountId: ").Append(AccountId).Append("\n");
            sb.Append("  ApiKey: ").Append(ApiKey).Append("\n");
            sb.Append("  BasicAuth: ").Append(BasicAuth).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
