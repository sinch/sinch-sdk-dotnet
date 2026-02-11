using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    public sealed class UpdateEmailRequest
    {
        [JsonPropertyName("phoneNumbers")]
        public List<NumberWithPermissions> PhoneNumbers { get; set; } = new();
    }
}
