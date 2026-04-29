using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<INumberSinchEvent> ParseEventAsync(Stream json, CancellationToken cancellationToken = default)
        {
            var result = await JsonSerializer.DeserializeAsync<NumberSinchEvent>(json, JsonSerializerOptions, cancellationToken);
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
