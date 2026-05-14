using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Voice.SinchEvents
{
    internal sealed class VoiceSinchEvents : IVoiceSinchEvents
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ApplicationSignedAuth? _applicationSignedAuth;
        private readonly ILoggerAdapter<IVoiceSinchEvents>? _logger;

        internal VoiceSinchEvents(
            JsonSerializerOptions jsonSerializerOptions,
            ApplicationSignedAuth? applicationSignedAuth = null,
            ILoggerAdapter<IVoiceSinchEvents>? logger = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
            _applicationSignedAuth = applicationSignedAuth;
            _logger = logger;
        }

        /// <inheritdoc />
        public VoiceSinchEvent ParseEvent(string json)
        {
            var result = JsonSerializer.Deserialize<VoiceSinchEvent>(json, _jsonSerializerOptions);
            if (result == null)
            {
                throw new InvalidOperationException("Deserialization of Voice Sinch Event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
            string body)
        {
            return AuthorizationHeaderValidation.Validate(method, path,
                headers.ToDictionary(x => x.Key, x => x.Value), body, ResolveApplicationSignedAuth(), _logger);
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, StringValues>> headers,
            string body)
        {
            var reHeaders = headers.ToDictionary(x => x.Key, x => (IEnumerable<string>)x.Value);
            return AuthorizationHeaderValidation.Validate(method, path, reHeaders, body,
                ResolveApplicationSignedAuth(), _logger);
        }

        /// <inheritdoc />
        public string SerializeResponse(CallEventResponse response)
        {
            return JsonSerializer.Serialize(response, _jsonSerializerOptions);
        }

        private ApplicationSignedAuth ResolveApplicationSignedAuth()
        {
            if (_applicationSignedAuth == null)
            {
                throw new InvalidOperationException(
                    "Voice application credentials are required to validate the authentication header.");
            }

            return _applicationSignedAuth;
        }
    }
}
