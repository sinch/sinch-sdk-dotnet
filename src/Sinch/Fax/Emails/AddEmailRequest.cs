using System.Text.Json.Serialization;

namespace Sinch.Fax.Emails
{
    internal sealed class AddEmailRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("phoneNumbers")]
        public string[] PhoneNumbers { get; set; } = null!;
    }
}
