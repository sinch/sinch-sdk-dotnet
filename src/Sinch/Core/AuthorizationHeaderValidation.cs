using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private static bool Validate<TLogger>(HttpMethod method, string path,
            Dictionary<string, StringValues> headers, string body, ApplicationSignedAuth applicationSignedAuth,
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
            var bytesBody = Encoding.UTF8.GetBytes(body);
            var contentType = headersCaseInsensitive.GetValueOrDefault("content-type");
            var timestamp = headersCaseInsensitive.GetValueOrDefault(timestampHeader, string.Empty);
            var calculatedSignature =
                applicationSignedAuth.GetSignedAuth(bytesBody, method.Method, path,
                    string.Join(':', timestampHeader, timestamp), contentType);
            var splitAuthHeader = authSignature.Split(' ');

            if (splitAuthHeader.FirstOrDefault() != "application")
            {
                logger?.LogDebug(
                    "Failed to validate auth header. Authorization header not starting with 'application'.");
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

        public static bool Validate<TLogger>(HttpMethod method, string path,
            Dictionary<string, IEnumerable<string>> headers, string body, ApplicationSignedAuth applicationSignedAuth,
            ILoggerAdapter<TLogger>? logger = null)
        {
            var reHeaders = headers.ToDictionary(x => x.Key, y => new StringValues(y.Value.ToArray()));

            return Validate(method, path, reHeaders, body, applicationSignedAuth, logger);
        }
    }
}
