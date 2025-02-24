using System;
using Sinch.Auth;

namespace Sinch.Verification
{
    public sealed class SinchVerificationConfiguration
    {
        public required string AppKey { get; init; }

        public required string AppSecret { get; init; }

        public string? UrlOverride { get; init; }

        /// <summary>
        ///     Only for e2e tests, not visible in public API, do not edit!
        /// </summary>
        internal AuthStrategy AuthStrategy { get; init; } = AuthStrategy.ApplicationSign;

        internal SinchVerificationConfiguration Validate()
        {
            if (string.IsNullOrEmpty(AppKey))
                throw new ArgumentNullException(nameof(AppKey), "The value should be present");

            if (string.IsNullOrEmpty(AppSecret))
                throw new ArgumentNullException(nameof(AppSecret), "The value should be present");

            return this;
        }
        
        public Uri ResolveUrl()
        {
            const string verificationApiUrl = "https://verification.api.sinch.com/";
            return new Uri(UrlOverride ?? verificationApiUrl);
        }
    }
}
