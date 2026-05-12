using System;
using System.Net.Http;
using System.Text.Json;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Verification.SinchEvents;

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

        /// <inheritdoc cref="IVerificationSinchEvents"/>
        IVerificationSinchEvents SinchEvents { get; }
    }

    internal sealed class SinchVerificationClient : ISinchVerificationClient
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions =
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private const string ConfigRequired =
            "VerificationConfiguration with AppKey and AppSecret is required to use Verification API methods. " +
            "Set VerificationConfiguration when creating SinchClient.";

        private readonly ISinchVerification? _verification;
        private readonly ISinchVerificationStatus? _verificationStatus;

        internal SinchVerificationClient(
            SinchVerificationConfiguration? config,
            string? verificationUrlOverride,
            LoggerFactory? loggerFactory,
            Func<HttpClient> httpClientAccessor)
        {
            var auth = new Lazy<ISinchAuth>(() =>
            {
                var currentConfig = config ??
                    throw new InvalidOperationException(ConfigRequired);

                if (string.IsNullOrEmpty(currentConfig.AppKey))
                    throw new ArgumentNullException(nameof(currentConfig.AppKey), "The value should be present");

                if (string.IsNullOrEmpty(currentConfig.AppSecret))
                    throw new ArgumentNullException(nameof(currentConfig.AppSecret), "The value should be present");

                if (currentConfig.AuthStrategy == AuthStrategy.ApplicationSign)
                    return new ApplicationSignedAuth(currentConfig.AppKey, currentConfig.AppSecret);

                return new BasicAuth(currentConfig.AppKey, currentConfig.AppSecret);
            });

            var verificationUrl = !string.IsNullOrEmpty(verificationUrlOverride)
                ? new Uri(verificationUrlOverride)
                : SinchUrlResolvers.ResolveVerificationUrl(config);

            Http? http = null;
            if (config != null)
            {
                http = new Http(auth, httpClientAccessor, loggerFactory?.Create<IHttp>(),
                    JsonNamingPolicy.CamelCase);

                _verification = new SinchVerification(loggerFactory?.Create<SinchVerification>(), verificationUrl,
                    http);
                _verificationStatus = new SinchVerificationStatus(
                    loggerFactory?.Create<SinchVerificationStatus>(), verificationUrl, http);
            }

            SinchEvents = new VerificationSinchEvents(
                http?.JsonSerializerOptions ?? DefaultJsonOptions,
                auth,
                loggerFactory?.Create<IVerificationSinchEvents>());
        }

        /// <inheritdoc />
        public ISinchVerification Verification =>
            _verification ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public ISinchVerificationStatus VerificationStatus =>
            _verificationStatus ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public IVerificationSinchEvents SinchEvents { get; }
    }
}
