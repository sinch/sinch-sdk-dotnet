using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Verification
{
    public interface ISinchVerificationClient
    {
        /// <summary>
        ///     Start new verification requests and report on existing verification requests.
        /// </summary>
        ISinchVerification Verification { get; }

        /// <summary>
        ///     Get the status of specific verification requests in the verification process.
        ///     Returns the status of pending and completed verifications.
        ///     You can retrieve the status of verification requests by using the ID of the request,
        ///     the phone number of the user being verified, or a custom reference string.
        /// </summary>
        ISinchVerificationStatus VerificationStatus { get; }

        /// <summary>
        ///     Validates callback request.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="headers"></param>
        /// <param name="contentHeaders"></param>
        /// <param name="body"></param>
        /// <param name="method"></param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(HttpMethod method, string path, HttpResponseHeaders headers,
            HttpContentHeaders contentHeaders,
            string body);

        /// <summary>
        ///     Validates callback request.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <param name="method"></param>
        /// <returns>True, if produced signature match with that of a header.</returns>
        bool ValidateAuthenticationHeader(HttpMethod method, string path,
            Dictionary<string, IEnumerable<string>> headers,
            string body);
    }

    internal sealed class SinchVerificationClient : ISinchVerificationClient
    {
        private readonly ILoggerAdapter<ISinchVerificationClient>? _logger;
        // TODO: 2.0 make it ApplicationSignedAuth
        private readonly ISinchAuth _applicationSignedAuth;

        internal SinchVerificationClient(Uri baseAddress, LoggerFactory? loggerFactory,
            IHttp http, ISinchAuth applicationSignedAuth)
        {
            _logger = loggerFactory?.Create<ISinchVerificationClient>();
            _applicationSignedAuth = applicationSignedAuth;
            Verification = new SinchVerification(loggerFactory?.Create<SinchVerification>(), baseAddress, http);
            VerificationStatus =
                new SinchVerificationStatus(loggerFactory?.Create<SinchVerificationStatus>(), baseAddress, http);
        }

        /// <inheritdoc />
        public ISinchVerification Verification { get; }

        /// <inheritdoc />
        public ISinchVerificationStatus VerificationStatus { get; }

        public bool ValidateAuthenticationHeader(HttpMethod method, string path, HttpResponseHeaders headers,
            HttpContentHeaders contentHeaders,
            string body)
        {
            // TODO: 2.0 remove when only application signed is allowed
            if (_applicationSignedAuth is not ApplicationSignedAuth auth)
            {
                throw new InvalidOperationException(
                    $"You can validate auth header only if {nameof(AuthStrategy.ApplicationSign)} is selected when creating {nameof(ISinchVerificationClient)}");
            }

            return AuthorizationHeaderValidation.Validate(method, path, headers, contentHeaders, body,
                auth, _logger);
        }

        public bool ValidateAuthenticationHeader(HttpMethod method, string path,
            Dictionary<string, IEnumerable<string>> headers, string body)
        {
            // TODO: 2.0 remove when only application signed is allowed
            if (_applicationSignedAuth is not ApplicationSignedAuth auth)
            {
                throw new InvalidOperationException(
                    $"You can validate auth header only if {nameof(AuthStrategy.ApplicationSign)} is selected when creating {nameof(ISinchVerificationClient)}");
            }
            return AuthorizationHeaderValidation.Validate(method, path, headers, body, auth,
                _logger);
        }
    }
}
