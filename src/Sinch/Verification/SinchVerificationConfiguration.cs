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
    }
}
