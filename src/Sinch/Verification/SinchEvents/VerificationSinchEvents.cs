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
            var result = JsonSerializer.Deserialize<IVerificationSinchEvent>(json, _jsonSerializerOptions);
            if (result is null)
            {
                _logger?.LogError(
                    "Failed to deserialize Verification Sinch Event. No matching event type found for payload: {json}",
                    json);
                throw new InvalidOperationException("Deserialization of Verification Sinch Event failed.");
            }

            return result;
        }

        public bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
            string body)
        {
            return AuthorizationHeaderValidation.Validate(
                method,
                path,
                headers.ToDictionary(h => h.Key, h => h.Value),
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
                headers.Select(h => new KeyValuePair<string, IEnumerable<string>>(h.Key, h.Value)),
                body);

        public string SerializeResponse(VerificationStartEventResponseBase response)
        {
            return JsonSerializer.Serialize(response, response.GetType(), _jsonSerializerOptions);
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
