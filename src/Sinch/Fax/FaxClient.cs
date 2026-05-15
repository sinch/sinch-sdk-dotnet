using System;
using System.Net.Http;
using System.Text.Json;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Fax.Emails;
using Sinch.Fax.Faxes;
using Sinch.Fax.Services;
using Sinch.Fax.SinchEvents;
using Sinch.Logger;

namespace Sinch.Fax
{
    /// <summary>
    ///     Our Fax API offers collision avoidance features, business integrations,
    ///     and a pay-as-you-go model suited for large scalability, designed with you, the developer, in mind.
    /// </summary>
    public interface ISinchFax
    {
        /// <inheritdoc cref="ISinchFaxFaxes" />
        ISinchFaxFaxes Faxes { get; }

        /// <inheritdoc cref="ISinchFaxEmails" />
        ISinchFaxEmails Emails { get; }

        /// <inheritdoc cref="ISinchFaxServices" />
        ISinchFaxServices Services { get; }

        /// <inheritdoc cref="IFaxSinchEvents"/>
        IFaxSinchEvents SinchEvents { get; }
    }

    internal sealed class FaxClient : ISinchFax
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions =
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        private const string ConfigRequired =
            "SinchUnifiedCredentials with ProjectId, KeyId, and KeySecret are required to use Fax API methods. " +
            "Set SinchUnifiedCredentials when creating SinchClient.";

        private readonly ISinchFaxFaxes? _faxes;
        private readonly ISinchFaxEmails? _emails;
        private readonly ISinchFaxServices? _services;

        internal FaxClient(
            SinchClientConfiguration config,
            string? faxUrlOverride,
            LoggerFactory? loggerFactory,
            Func<HttpClient> httpClientAccessor)
        {
            var credentials = config.SinchUnifiedCredentials;
            Http? http = null;

            if (credentials != null)
            {
                var oauthUrl = SinchUrlResolvers.ResolveAuthUrl(config.SinchOAuthConfiguration);
                var auth = new OAuth(
                    credentials.KeyId ?? string.Empty,
                    credentials.KeySecret ?? string.Empty,
                    httpClientAccessor,
                    loggerFactory?.Create<OAuth>(),
                    oauthUrl);

                http = new Http(
                    new Lazy<ISinchAuth>(auth),
                    httpClientAccessor,
                    loggerFactory?.Create<IHttp>(),
                    JsonNamingPolicy.CamelCase);

                var faxUrl = !string.IsNullOrEmpty(faxUrlOverride)
                    ? new Uri(faxUrlOverride)
                    : SinchUrlResolvers.ResolveFaxUrl(config.FaxConfiguration);

                var projectId = credentials.ProjectId ?? string.Empty;
                _faxes = new FaxesClient(projectId, faxUrl, loggerFactory?.Create<ISinchFaxFaxes>(), http);
                _services = new ServicesClient(projectId, faxUrl, loggerFactory?.Create<ISinchFaxServices>(), http);
                _emails = new EmailsClient(projectId, faxUrl, loggerFactory?.Create<ISinchFaxEmails>(), http);
            }

            SinchEvents = new FaxSinchEvents(
                http?.JsonSerializerOptions ?? DefaultJsonOptions,
                loggerFactory?.Create<IFaxSinchEvents>());
        }

        /// <summary>Test-only constructor that accepts a pre-built <see cref="IHttp"/> instance.</summary>
        internal FaxClient(string projectId, Uri baseAddress, LoggerFactory? loggerFactory, IHttp http)
        {
            _faxes = new FaxesClient(projectId, baseAddress, loggerFactory?.Create<ISinchFaxFaxes>(), http);
            _services = new ServicesClient(projectId, baseAddress, loggerFactory?.Create<ISinchFaxServices>(), http);
            _emails = new EmailsClient(projectId, baseAddress, loggerFactory?.Create<ISinchFaxEmails>(), http);
            SinchEvents = new FaxSinchEvents(
                http.JsonSerializerOptions,
                loggerFactory?.Create<IFaxSinchEvents>());
        }

        /// <inheritdoc />
        public ISinchFaxFaxes Faxes =>
            _faxes ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public ISinchFaxEmails Emails =>
            _emails ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public ISinchFaxServices Services =>
            _services ?? throw new InvalidOperationException(ConfigRequired);

        /// <inheritdoc />
        public IFaxSinchEvents SinchEvents { get; }
    }
}
