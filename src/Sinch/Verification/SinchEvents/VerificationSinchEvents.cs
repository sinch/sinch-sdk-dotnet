using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Verification.SinchEvents
{
    internal sealed class VerificationSinchEvents : IVerificationSinchEvents
    {
        private readonly ApplicationSignedAuth? _applicationSignedAuth;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILoggerAdapter<IVerificationSinchEvents>? _logger;

        internal VerificationSinchEvents(
            JsonSerializerOptions jsonSerializerOptions,
            ApplicationSignedAuth? applicationSignedAuth = null,
            ILoggerAdapter<IVerificationSinchEvents>? logger = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
            _applicationSignedAuth = applicationSignedAuth;
            _logger = logger;
        }

        public IVerificationSinchEvent ParseEvent(string json)
        {
            using var document = JsonDocument.Parse(json);
            if (!document.RootElement.TryGetProperty("event", out var eventTypeProperty) ||
                eventTypeProperty.ValueKind != JsonValueKind.String)
            {
                throw new JsonException("Verification Sinch Event payload is missing the event discriminator.");
            }

            var eventType = eventTypeProperty.GetString();
            return eventType switch
            {
                "VerificationRequestEvent" => DeserializeEvent<VerificationRequestEvent>(json, eventType),
                "VerificationResultEvent" => DeserializeEvent<VerificationResultEvent>(json, eventType),
                _ => throw new JsonException($"Unknown Verification Sinch Event type '{eventType}'.")
            };
        }

        public bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            Dictionary<string, IEnumerable<string>> headers,
            string body)
        {
            if (_applicationSignedAuth is null)
            {
                throw new InvalidOperationException(
                    "Verification application credentials are required to validate the authentication header.");
            }

            return AuthorizationHeaderValidation.Validate(
                method,
                path,
                headers,
                body,
                _applicationSignedAuth,
                _logger);
        }

        public bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, StringValues>> headers,
            string body) =>
            ValidateAuthenticationHeader(
                method,
                path,
                headers.ToDictionary(
                    header => header.Key,
                    header => header.Value
                        .Where(value => value is not null)
                        .Select(value => value!),
                    StringComparer.OrdinalIgnoreCase),
                body);

        public string SerializeResponse(RequestEventResponseBase response)
        {
            return JsonSerializer.Serialize(response, response.GetType(), _jsonSerializerOptions);
        }

        private TEvent DeserializeEvent<TEvent>(string json, string? eventType)
            where TEvent : class, IVerificationSinchEvent
        {
            var result = JsonSerializer.Deserialize<TEvent>(json, _jsonSerializerOptions);
            if (result is null)
            {
                _logger?.LogError(
                    "Failed to deserialize Verification Sinch Event of type {eventType}. Payload: {json}",
                    eventType,
                    json);
                throw new InvalidOperationException("Deserialization of Verification Sinch Event failed.");
            }

            return result;
        }
    }
}