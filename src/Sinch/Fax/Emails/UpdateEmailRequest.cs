using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    internal sealed class UpdateEmailRequest
    {
        [JsonPropertyName("phoneNumbers")]
        public List<string> PhoneNumbers { get; set; } = new();
    }
}
