using System.Text;

namespace Sinch.Conversation.Webhooks
{
    /// <summary>
    ///     Optional. Used for OAuth2 authentication.
    /// </summary>
    public sealed class ClientCredentials
    {
        /// <summary>
        ///     The Client ID that will be used in the OAuth2 Client Credentials flow.
        /// </summary>

        public required string ClientId { get; set; }



        /// <summary>
        ///     The Client Secret that will be used in the OAuth2 Client Credentials flow.
        /// </summary>

        public required string ClientSecret { get; set; }



        /// <summary>
        ///     The endpoint that will be used in the OAuth2 Client Credentials flow. Expected to return a JSON with an access
        ///     token and &#x60;expires_in&#x60; value (in seconds). The &#x60;expires_in&#x60; value, which must be a minimum of
        ///     30 seconds and a maximum of 3600 seconds, is how long Sinch will save the access token before asking for a new one.
        /// </summary>

        public required string Endpoint { get; set; }



        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ClientCredentials {\n");
            sb.Append("  ClientId: ").Append(ClientId).Append("\n");
            sb.Append("  Endpoint: ").Append(Endpoint).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
