using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    internal class UpdateEmailRequest
    {
        [JsonPropertyName("phoneNumbers")]
        public List<string> PhoneNumbers { get; set; } = new();
    }
}
