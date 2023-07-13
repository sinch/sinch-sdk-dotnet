using System.Text.Json.Serialization;

namespace Sinch.Auth
{
    internal class AuthApiError
    {
        public string Error { get; set; }

        [JsonPropertyName("error_verbose")]
        public string ErrorVerbose { get; set; }

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }

        [JsonPropertyName("error_hint")]
        public string ErrorHint { get; set; }
    }
}
