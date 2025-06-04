using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Primitives;
using Sinch.Auth;
using Sinch.Logger;

namespace Sinch.Core
{
    /// <summary>
    ///     Validates authentication header for Voice And Verification webhooks <br/>
    ///     For details, [see](https://developers.sinch.com/docs/verification/api-reference/authentication/callback-signed-request/)
    /// </summary>
    internal static class AuthorizationHeaderValidation
    {
        // TODO: refactor in 2.0 and remove JsonObject variant
        // kept for backward compatibility
        public static bool Validate<TLogger>(HttpMethod method, string path,
            Dictionary<string, StringValues> headers, JsonObject body, ApplicationSignedAuth applicationSignedAuth,
            string? rawBody = null,
            ILoggerAdapter<TLogger>? logger = null)
        {
            var headersCaseInsensitive =
                new Dictionary<string, StringValues>(headers, StringComparer.InvariantCultureIgnoreCase);

            if (!headersCaseInsensitive.TryGetValue("authorization", out var authHeaderValue))
            {
                logger?.LogDebug("Failed to validate auth header. Authorization header is missing.");
                return false;
            }

            if (authHeaderValue.Count == 0)
            {
                logger?.LogDebug("Failed to validate auth header. Authorization header values is missing.");
                return false;
            }

            var authSignature = authHeaderValue.FirstOrDefault();
            if (authSignature == null)
            {
                logger?.LogDebug("Failed to validate auth header. Authorization header value is null.");
                return false;
            }

            const string timestampHeader = "x-timestamp";
            var bytesBody =
                rawBody != null
                    ? Encoding.UTF8.GetBytes(rawBody)
                    : JsonSerializer.SerializeToUtf8Bytes(body);
            var contentType = headersCaseInsensitive.GetValueOrDefault("content-type");
            var timestamp = headersCaseInsensitive.GetValueOrDefault(timestampHeader, string.Empty);
            var calculatedSignature =
                applicationSignedAuth.GetSignedAuth(bytesBody, method.Method, path,
                    string.Join(':', timestampHeader, timestamp), contentType);
            var splitAuthHeader = authSignature.Split(' ');

            if (splitAuthHeader.FirstOrDefault() != "application")
            {
                logger?.LogDebug(
                    "Failed to validate auth header. Authorization header not starting from 'application'.");
                return false;
            }

            var signature = splitAuthHeader.Skip(1).FirstOrDefault();

            if (string.IsNullOrEmpty(signature))
            {
                logger?.LogDebug("Failed to validate auth header. Signature is missing from the header.");
                return false;
            }

            logger?.LogDebug("{CalculatedSignature}", calculatedSignature);
            logger?.LogDebug("{AuthorizationSignature}", signature);

            var isValidSignature = string.Equals(signature, calculatedSignature, StringComparison.Ordinal);
            logger?.LogInformation("The signature was validated with {success}", isValidSignature);
            return isValidSignature;
        }

        public static bool Validate<TLogger>(HttpMethod method, string path, HttpResponseHeaders headers,
            HttpContentHeaders contentHeaders,
            string body, ApplicationSignedAuth applicationSignedAuth, ILoggerAdapter<TLogger>? logger = null)
        {
            // apparently, `HttpResponseHeaders` does not contains `Content-Type`, which sits in HttpContentHeaders
            var headersReformat = headers.ToDictionary(x => x.Key, y => new StringValues(y.Value.ToArray()));
            var contentHeadersReformat =
                contentHeaders.ToDictionary(x => x.Key, y => new StringValues(y.Value.ToArray()));
            var allHeaders = headersReformat.Concat(contentHeadersReformat).ToDictionary(x => x.Key, y => y.Value);
            var json = JsonNode.Parse(body);
            if (json == null)
            {
                throw new InvalidOperationException($"Parameter {nameof(body)} is not a valid json.");
            }

            return Validate(method, path, allHeaders, json.AsObject(), applicationSignedAuth, body, logger);
        }

        public static bool Validate<TLogger>(HttpMethod method, string path,
            Dictionary<string, IEnumerable<string>> headers, string body, ApplicationSignedAuth applicationSignedAuth,
            ILoggerAdapter<TLogger>? logger = null)
        {
            var json = JsonNode.Parse(body);
            if (json == null)
            {
                throw new InvalidOperationException($"Parameter {nameof(body)} is not a valid json.");
            }

            var reHeaders = headers.ToDictionary(x => x.Key, y => new StringValues(y.Value.ToArray()));
            return Validate(method, path, reHeaders, json.AsObject(), applicationSignedAuth, body, logger);
        }
    }
}
