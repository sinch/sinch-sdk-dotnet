using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    public sealed class ListEmailsResponse<T> : PagedResponse
    {
        /// <summary>
        ///     List of emails.
        /// </summary>
        [JsonPropertyName("emails")]
        public List<T> Emails { get; set; } = new();
    }
}
