using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sinch.Fax.Emails;

namespace Sinch.Fax.Services
{
    internal sealed class ListFaxServiceNumbersResponse : PagedResponse
    {
        [JsonPropertyName("numbers")]
        public List<ServicePhoneNumber> Numbers { get; set; } = new();
    }
}
