using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    public sealed class ListEmailAddressesResponse : PagedResponse
    {
        /// <summary>
        ///     List of email addresses as strings.
        /// </summary>
        [JsonPropertyName("emails")]
        public List<string> EmailAddresses { get; set; } = new();
    }
}
