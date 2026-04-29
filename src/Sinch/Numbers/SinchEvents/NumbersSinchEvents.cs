using System.Net.Http.Headers;
using System.Text.Json;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Numbers.SinchEvents
{
    internal sealed class NumbersSinchEvents(JsonSerializerOptions jsonSerializerOptions,
        ILoggerAdapter<INumbersSinchEvents>? logger = null) : INumbersSinchEvents
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = jsonSerializerOptions;

        public INumberSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<NumberSinchEvent>(json, JsonSerializerOptions);
            if (result == null)
            {
                logger?.LogError("Failed to deserialize Numbers Sinch event.");
                throw new System.InvalidOperationException("Deserialization of Numbers Sinch event failed");
            }

            return result;
        }

        public bool ValidateAuthenticationHeader(string hmacSecret, string json, string signatureHeaderValue)
        {
            return HeaderValidation.ValidateAuthHeader(hmacSecret, json, signatureHeaderValue);
        }

        public bool ValidateAuthenticationHeader(string hmacSecret, string json, HttpHeaders headers)
        {
            return HeaderValidation.ValidateAuthHeader(hmacSecret, json, headers);
        }
    }
}
