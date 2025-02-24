using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    public sealed class ListNumbersResponse : PagedResponse
    {
        [JsonPropertyName("phoneNumbers")]
        public List<ServicePhoneNumber> PhoneNumbers { get; set; } = new();
    }
}
