using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Primitives;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Voice.Applications;
using Sinch.Voice.Callouts;
using Sinch.Voice.Calls;
using Sinch.Voice.Conferences;

namespace Sinch.Voice
{
    public interface ISinchVoiceClient
    {
        /// <summary>
        ///     A callout is a call made to a phone number or app using the API.
        /// </summary>
        ISinchVoiceCallout Callouts { get; }

        /// <summary>
        ///     Using the Calls endpoint, you can manage on-going calls or retrieve information about a call.
        /// </summary>
        ISinchVoiceCalls Calls { get; }

        /// <summary>
        ///     Using the Conferences endpoint, you can perform tasks like retrieving information about an on-going conference,
        ///     muting or unmuting participants, or removing participants from a conference.
        /// </summary>
        ISinchVoiceConferences Conferences { get; }

        /// <summary>
        ///     You can use the API to manage features of applications in your project.
        /// </summary>
        ISinchVoiceApplications Applications { get; }

        /// <summary>
        ///     Validates callback request.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <param name="method"></param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(HttpMethod method, string path, Dictionary<string, StringValues> headers,
            JsonObject body);
    }

    /// <inheritdoc />
    internal class SinchVoiceClient : ISinchVoiceClient
    {
        private readonly ApplicationSignedAuth _applicationSignedAuth;
        private readonly ILoggerAdapter<ISinchVoiceClient>? _logger;

        public SinchVoiceClient(Uri baseAddress, LoggerFactory? loggerFactory,
            IHttp http, ApplicationSignedAuth applicationSignedAuth)
        {
            _applicationSignedAuth = applicationSignedAuth;
            _logger = loggerFactory?.Create<ISinchVoiceClient>();
            Callouts = new SinchCallout(loggerFactory?.Create<ISinchVoiceCallout>(), baseAddress, http);
            Calls = new SinchCalls(loggerFactory?.Create<ISinchVoiceCalls>(), baseAddress, http);
            Conferences = new SinchConferences(loggerFactory?.Create<ISinchVoiceConferences>(), baseAddress, http);
            Applications = new SinchApplications(loggerFactory?.Create<ISinchVoiceApplications>(), baseAddress, http);
        }

        /// <inheritdoc />
        public ISinchVoiceCallout Callouts { get; }

        /// <inheritdoc />
        public ISinchVoiceCalls Calls { get; }

        /// <inheritdoc />
        public ISinchVoiceConferences Conferences { get; }

        /// <inheritdoc />
        public ISinchVoiceApplications Applications { get; }

        public bool ValidateAuthenticationHeader(HttpMethod method, string path,
            Dictionary<string, StringValues> headers, JsonObject body)
        {
            var headersCaseInsensitive =
                new Dictionary<string, StringValues>(headers, StringComparer.InvariantCultureIgnoreCase);

            if (!headersCaseInsensitive.TryGetValue("authorization", out var authHeaderValue))
            {
                _logger?.LogDebug("Failed to validate auth header. Authorization header is missing.");
                return false;
            }

            if (authHeaderValue.Count == 0)
            {
                _logger?.LogDebug("Failed to validate auth header. Authorization header values is missing.");
                return false;
            }

            var authSignature = authHeaderValue.FirstOrDefault();
            if (authSignature == null)
            {
                _logger?.LogDebug("Failed to validate auth header. Authorization header value is null.");
                return false;
            }

            const string timestampHeader = "x-timestamp";
            var bytesBody = JsonSerializer.SerializeToUtf8Bytes(body);
            var contentType = headersCaseInsensitive.GetValueOrDefault("content-type");
            var timestamp = headersCaseInsensitive.GetValueOrDefault(timestampHeader, string.Empty);
            var calculatedSignature =
                _applicationSignedAuth.GetSignedAuth(bytesBody, method.Method, path,
                    string.Join(':', timestampHeader, timestamp), contentType);
            var splitAuthHeader = authSignature.Split(' ');

            if (splitAuthHeader.FirstOrDefault() != "application")
            {
                _logger?.LogDebug(
                    "Failed to validate auth header. Authorization header not starting from 'application'.");
                return false;
            }

            var signature = splitAuthHeader.Skip(1).FirstOrDefault();

            if (string.IsNullOrEmpty(signature))
            {
                _logger?.LogDebug("Failed to validate auth header. Signature is missing from the header.");
                return false;
            }

            _logger?.LogDebug("{CalculatedSignature}", calculatedSignature);
            _logger?.LogDebug("{AuthorizationSignature}", signature);

            var isValidSignature = string.Equals(signature, calculatedSignature, StringComparison.Ordinal);
            _logger?.LogInformation("The signature was validated with {success}", isValidSignature);
            return isValidSignature;
        }
    }
}
