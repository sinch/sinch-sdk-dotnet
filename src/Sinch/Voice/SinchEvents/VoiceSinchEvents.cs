using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
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
        public VoiceSinchEvent ParseEvent(JsonNode json)
        {
            var result = json.Deserialize<VoiceSinchEvent>(_jsonSerializerOptions);
            if (result == null)
            {
                throw new InvalidOperationException("Deserialization of Voice Sinch Event failed");
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<VoiceSinchEvent> ParseEventAsync(Stream json,
            CancellationToken cancellationToken = default)
        {
            var result =
                await JsonSerializer.DeserializeAsync<VoiceSinchEvent>(json, _jsonSerializerOptions,
                    cancellationToken);
            if (result == null)
            {
                throw new InvalidOperationException("Deserialization of Voice Sinch Event failed");
            }

            return result!;
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
            string body)
        {
            EnsureCredentials();
            return AuthorizationHeaderValidation.Validate(method, path,
                headers.ToDictionary(x => x.Key, x => x.Value), body, _applicationSignedAuth!, _logger);
        }

        /// <inheritdoc />
        public bool ValidateAuthenticationHeader(
            HttpMethod method,
            string path,
            IEnumerable<KeyValuePair<string, StringValues>> headers,
            string body)
        {
            EnsureCredentials();
            var reHeaders = headers.ToDictionary(x => x.Key, x => (IEnumerable<string>)x.Value);
            return AuthorizationHeaderValidation.Validate(method, path, reHeaders, body, _applicationSignedAuth!,
                _logger);
        }

        /// <inheritdoc />
        public string SerializeResponse(CallEventResponse response)
        {
            return JsonSerializer.Serialize(response, _jsonSerializerOptions);
        }

        private void EnsureCredentials()
        {
            if (_applicationSignedAuth == null)
            {
                throw new InvalidOperationException(
                    "Voice application credentials are required to validate the authentication header.");
            }
        }
    }
}
