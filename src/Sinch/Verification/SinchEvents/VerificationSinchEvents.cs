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
        private readonly Lazy<ISinchAuth>? _auth;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILoggerAdapter<IVerificationSinchEvents>? _logger;

        internal VerificationSinchEvents(
            JsonSerializerOptions jsonSerializerOptions,
            Lazy<ISinchAuth>? auth = null,
            ILoggerAdapter<IVerificationSinchEvents>? logger = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
            _auth = auth;
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

            if (eventTypeProperty.ValueEquals("VerificationRequestEvent"))
            {
                return DeserializeEvent<VerificationStartEvent>(json, "VerificationRequestEvent");
            }

            if (eventTypeProperty.ValueEquals("VerificationResultEvent"))
            {
                return DeserializeEvent<VerificationResultEvent>(json, "VerificationResultEvent");
            }

            throw new JsonException(
                $"Unknown Verification Sinch Event type '{eventTypeProperty.GetString()}'.");
        }

        public bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            Dictionary<string, IEnumerable<string>> headers,
            string body)
        {
            return AuthorizationHeaderValidation.Validate(
                method,
                path,
                headers,
                body,
                ResolveApplicationSignedAuth(),
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

        public string SerializeResponse(VerificationStartEventResponseBase response)
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

        private ApplicationSignedAuth ResolveApplicationSignedAuth()
        {
            if (_auth is null)
            {
                throw CreateMissingVerificationCredentialsException();
            }

            try
            {
                if (_auth.Value is ApplicationSignedAuth applicationSignedAuth)
                {
                    return applicationSignedAuth;
                }
            }
            catch (InvalidOperationException exception)
            {
                throw CreateMissingVerificationCredentialsException(exception);
            }
            catch (ArgumentNullException exception)
            {
                throw CreateMissingVerificationCredentialsException(exception);
            }

            throw CreateMissingVerificationCredentialsException();
        }

        private static InvalidOperationException CreateMissingVerificationCredentialsException(
            Exception? innerException = null)
        {
            return new InvalidOperationException(
                "Verification application credentials are required to validate the authentication header.",
                innerException);
        }
    }
}
