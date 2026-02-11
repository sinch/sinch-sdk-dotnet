using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    /// <summary>
    /// A phone number and its permissions.
    /// </summary>
    public sealed class NumberWithPermissions
    {
        /// <summary>
        ///     A phone number in E.164 format, including the leading '+'.
        /// </summary>
        [JsonPropertyName("number")]
        public string? Number { get; set; }

        /// <summary>
        ///     Allows you to set permissions for sending and receiving faxes to this email/phone number combination.
        /// </summary>
        [JsonPropertyName("permissions")]
        public EmailPermissions? Permissions { get; set; }
    }
}
