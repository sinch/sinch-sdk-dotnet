using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sinch.Fax.Services
{
    public sealed class ListServicesResponse : PagedResponse
    {
        [JsonPropertyName("services")]
        public List<Service> Services { get; set; } = new();
    }
}
