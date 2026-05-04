using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Sinch.Auth;
using Sinch.Conversation;
using Sinch.Core;
using Sinch.Fax;
using Sinch.Logger;
using Sinch.Numbers;
using Sinch.SMS;
using Sinch.Verification;
using Sinch.Voice;

namespace Sinch
{
    public interface ISinchClient : IDisposable
    {
        /// <summary>
        ///     An OAuth2.0 functionality for an SDK in case you want to fetch tokens.
        ///     <see href="https://developers.sinch.com/docs/numbers/api-reference/authentication/oauth/">
        ///         Learn more.
        ///     </see>
        /// </summary>
        public ISinchAuth Auth { get; }

        /// <summary>
        ///     The Numbers API enables you to search for, view, and activate numbers.
        ///     It's considered a precursor to other APIs in the Sinch product family.
        ///     The numbers API can be used in tandem with any of our APIs that perform messaging or calling.
        ///     Once you have activated your numbers, you can then use the various other APIs, such as
        ///     <see href="https://developers.sinch.com/docs/sms/">
        ///         SMS
        ///     </see>
        ///     or
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/">
        ///         Voice
        ///     </see>
        ///     , to assign and use those numbers. <br /><br />
        ///     <see href="https://developers.sinch.com/docs/numbers/api-reference/">
        ///         Learn more.
        ///     </see>
        /// </summary>
        public ISinchNumbers Numbers { get; }

        /// <summary>
        ///     Send and receive SMS through a single connection for timely and cost-efficient communications using
        ///     the Sinch SMS API.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/sms/getting-started/">
        ///         Learn more.
        ///     </see>
        /// </summary>
        public ISinchSms Sms { get; }

        /// <summary>
        ///     Send and receive messages globally over SMS, RCS, WhatsApp, Viber Business,
        ///     Facebook messenger and other popular channels using the Sinch Conversation API.<br /><br />
        ///     The Conversation API endpoint uses built-in transcoding to give you the power of conversation
        ///     across all supported channels and, if required, full control over channel specific features.<br /><br />
        ///     <see href="https://developers.sinch.com/docs/conversation/api-reference/">Learn more.</see>
        /// </summary>
        public ISinchConversation Conversation { get; }

        /// <summary>
        ///     Sinch Fax. Currently, in closed Beta support.
        ///     You can always reach us at <see href="faxbetasupport@sinch.com">Fax API Closed Beta Support</see>.
        /// </summary>
        public ISinchFax Fax { get; }

        /// <summary>
        ///     Verify users with SMS, flash calls (missed calls), a regular call, or data verification.
        ///     This document serves as a user guide and documentation on how to use the Sinch Verification REST APIs.
        ///     <br /><br />
        ///     The Sinch Verification API is used to verify mobile phone numbers.
        ///     It's consumed by the Sinch Verification SDK, but it can also be used by any backend or client directly.
        ///     <br /><br />
        ///     The Sinch service uses four different verification methods:
        ///     <list type="bullet">
        ///         <item>
        ///             SMS : Sending an SMS message with a PIN code
        ///         </item>
        ///         <item>
        ///             FlashCall : Placing a flashcall (missed call) and detecting the incoming calling number (CLI)
        ///         </item>
        ///         <item>
        ///             Phone Call : Placing a PSTN call to the user's phone and playing a message containing the code
        ///         </item>
        ///         <item>
        ///             Data : By accessing internal infrastructure of mobile carriers to verify
        ///             if given verification attempt was originated from device with matching phone number <br />
        ///             Note: If you want to use data verification, please contact your account manager.
        ///         </item>
        ///     </list>
        /// </summary>
        /// <returns></returns>
        public ISinchVerificationClient Verification { get; }

        /// <summary>
        ///     When using Sinch for voice calling, the Sinch dashboard works as a big telephony switch.
        ///     The dashboard handles incoming phone calls (also known as incoming call “legs”),
        ///     sets up outgoing phone calls (or outgoing call “legs”), and bridges the two.
        ///     The incoming call leg may come in over a data connection
        ///     (from a smartphone or web application using the Sinch SDKs)
        ///     or through a local phone number (from the PSTN network).
        ///     Similarly, the outgoing call leg can be over
        ///     data (to another smartphone or web application using the Sinch SDKs) or the PSTN network.
        /// </summary>
        /// <returns></returns>
        public ISinchVoiceClient Voice { get; }
    }

    public sealed class SinchClient : ISinchClient
    {
        private bool _disposed;
        private readonly LoggerFactory? _loggerFactory;
        private readonly Func<HttpClient> _httpClientAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Lazy<IHttp> _httpCamelCase;
        private readonly Lazy<IHttp> _httpSnakeCase;
        private readonly SinchClientConfiguration _sinchClientConfiguration;
        private readonly Lazy<ISinchAuth> _sinchOauth;
        private readonly Lazy<ISinchNumbers> _numbers;
        private readonly Lazy<ISinchSms> _sms;
        private readonly ILoggerAdapter<ISinchClient>? _logger;
        private readonly Lazy<ISinchFax> _fax;
        private readonly Lazy<ISinchVerificationClient> _verification;
        private readonly Lazy<ISinchConversation> _conversation;
        private readonly Lazy<ISinchVoiceClient> _voice;

        /// <inheritdoc />
        public ISinchNumbers Numbers => _numbers.Value;

        /// <inheritdoc />
        public ISinchSms Sms => _sms.Value;

        /// <inheritdoc />
        public ISinchConversation Conversation => _conversation.Value;

        /// <inheritdoc />
        public ISinchAuth Auth => _sinchOauth.Value;

        /// <inheritdoc />
        public ISinchFax Fax => _fax.Value;

        /// <inheritdoc/>
        public ISinchVerificationClient Verification => _verification.Value;

        /// <inheritdoc />
        public ISinchVoiceClient Voice => _voice.Value;

        public SinchClient(SinchClientConfiguration clientConfiguration)
        {
            _sinchClientConfiguration = clientConfiguration;

            if (clientConfiguration.SinchOptions?.LoggerFactory is not null)
                _loggerFactory = new LoggerFactory(clientConfiguration.SinchOptions.LoggerFactory);

            _logger = _loggerFactory?.Create<ISinchClient>();
            _logger?.LogInformation("Initializing SinchClient...");

            _httpClientFactory = _sinchClientConfiguration.SinchOptions?.HttpClientFactory
                ?? new DefaultHttpClientFactory(_sinchClientConfiguration.SinchOptions?.HttpClientHandlerConfiguration);

            _httpClientAccessor = () => _httpClientFactory.CreateClient("SinchClient");

            _httpCamelCase = new Lazy<IHttp>(InitHttpCamelCase, isThreadSafe: true);

            _httpSnakeCase = new Lazy<IHttp>(InitHttpSnakeCase, isThreadSafe: true);

            _sinchOauth = new Lazy<ISinchAuth>(InitOauth, isThreadSafe: true);

            _numbers = new Lazy<ISinchNumbers>(InitNumbers, isThreadSafe: true);

            _sms = new Lazy<ISinchSms>(InitSms, isThreadSafe: true);

            _conversation = new Lazy<ISinchConversation>(InitConversation, isThreadSafe: true);

            _fax = new Lazy<ISinchFax>(InitFax, isThreadSafe: true);

            _verification = new Lazy<ISinchVerificationClient>(InitVerification, isThreadSafe: true);

            _voice = new Lazy<ISinchVoiceClient>(InitVoice, isThreadSafe: true);

            _logger?.LogInformation("SinchClient initialized.");
        }

        private ISinchAuth InitOauth()
        {
            var unifiedCredentials = ValidateUnifiedCredentials();

            var oauthBaseUrl = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.AuthUrl,
                () => SinchUrlResolvers.ResolveAuthUrl(_sinchClientConfiguration.SinchOAuthConfiguration));

            var auth = new OAuth(unifiedCredentials.KeyId, unifiedCredentials.KeySecret, _httpClientAccessor,
                _loggerFactory?.Create<OAuth>(),
                oauthBaseUrl
            );

            return auth;
        }

        private ISinchVoiceClient InitVoice()
        {
            var config = _sinchClientConfiguration.VoiceConfiguration ??
                throw new InvalidOperationException($"{nameof(SinchVoiceConfiguration)} is not set.");

            if (string.IsNullOrEmpty(config.AppKey))
                throw new ArgumentNullException(nameof(config.AppKey), "The value should be present");

            if (string.IsNullOrEmpty(config.AppSecret))
                throw new ArgumentNullException(nameof(config.AppSecret), "The value should be present");

            ISinchAuth auth = new ApplicationSignedAuth(config.AppKey, config.AppSecret);

            var http = new Http(new Lazy<ISinchAuth>(auth), _httpClientAccessor, _loggerFactory?.Create<IHttp>(),
                JsonNamingPolicy.CamelCase);

            var voiceUrl = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.VoiceUrl,
                () => SinchUrlResolvers.ResolveVoiceUrl(config));

            var voiceAppMgmtUrl = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.VoiceApplicationManagementUrl,
                () => SinchUrlResolvers.ResolveVoiceApplicationManagementUrl(config));

            return new SinchVoiceClient(
                voiceUrl,
                _loggerFactory, http, (auth as ApplicationSignedAuth)!,
                voiceAppMgmtUrl);
        }

        private ISinchVerificationClient InitVerification()
        {
            var config = _sinchClientConfiguration.VerificationConfiguration ??
                throw new InvalidOperationException($"{nameof(SinchVerificationConfiguration)} is not set.");

            if (string.IsNullOrEmpty(config.AppKey))
                throw new ArgumentNullException(nameof(config.AppKey), "The value should be present");

            if (string.IsNullOrEmpty(config.AppSecret))
                throw new ArgumentNullException(nameof(config.AppSecret), "The value should be present");

            ISinchAuth auth;
            if (config.AuthStrategy == AuthStrategy.ApplicationSign)
                auth = new ApplicationSignedAuth(config.AppKey, config.AppSecret);
            else
                auth = new BasicAuth(config.AppKey, config.AppSecret);

            var http = new Http(new Lazy<ISinchAuth>(auth), _httpClientAccessor, _loggerFactory?.Create<IHttp>(),
                JsonNamingPolicy.CamelCase);

            var verificationUrl = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.VerificationUrl,
                () => SinchUrlResolvers.ResolveVerificationUrl(config));

            return new SinchVerificationClient(verificationUrl, _loggerFactory, http, (auth as ApplicationSignedAuth)!);
        }

        private ISinchFax InitFax()
        {
            var unifiedCredentials = ValidateUnifiedCredentials();

            var faxConfig = _sinchClientConfiguration.FaxConfiguration;

            var faxUrl = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.FaxUrl,
                () => SinchUrlResolvers.ResolveFaxUrl(faxConfig));

            return new FaxClient(unifiedCredentials.ProjectId, faxUrl, _loggerFactory, _httpCamelCase.Value);
        }

        private ISinchConversation InitConversation()
        {
            var conversationConfig = _sinchClientConfiguration.ConversationConfiguration;

            // TODO! Need to be refactored when the EventsDestination support will be addressed: https://sinchenterprise.atlassian.net/browse/DEVEXP-1311
            if (conversationConfig.Region == null)
                throw new InvalidOperationException(
                    $"{nameof(SinchConversationConfiguration)}.{nameof(SinchConversationConfiguration.Region)} is required. " +
                    $"Set it to one of the values in {nameof(ConversationRegion)}, e.g. {nameof(ConversationRegion)}.{nameof(ConversationRegion.Us)}.");

            var conversationBaseAddress = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.ConversationUrl,
                () => SinchUrlResolvers.ResolveConversationUrl(conversationConfig));

            var templatesBaseAddress = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.TemplatesUrl,
                () => SinchUrlResolvers.ResolveConversationTemplateUrl(conversationConfig));

            return new SinchConversationClient(
                _sinchClientConfiguration.SinchUnifiedCredentials
                    ?.ProjectId!,
                // unified credentials, alongside projectId, will be validated as part of lazy call to http
                // this is needed for working of Conversation.Webhooks.ParseEvent() to be accessible, without providing
                // SinchUnifiedCredentials, the design regarding just a static method for this is still in discussion.
                conversationBaseAddress,
                templatesBaseAddress,
                _loggerFactory,
                _httpSnakeCase);
        }

        private ISinchNumbers InitNumbers()
        {
            var numbersBaseUrl = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.NumbersUrl,
                () => SinchUrlResolvers.ResolveNumbersUrl(_sinchClientConfiguration.NumbersConfiguration));

            var projectId = _sinchClientConfiguration.SinchUnifiedCredentials?.ProjectId ?? string.Empty;

            return new Numbers.Numbers(projectId,
                numbersBaseUrl,
                _loggerFactory, _httpCamelCase.Value);
        }

        private SmsClient InitSms()
        {
            var sinchSmsConfiguration = _sinchClientConfiguration.SmsConfiguration;

            if (sinchSmsConfiguration.ServicePlanIdConfiguration != null)
            {
                var servicePlanIdConfig = sinchSmsConfiguration.ServicePlanIdConfiguration;

                if (servicePlanIdConfig.ServicePlanIdRegion == null)
                    throw new InvalidOperationException(
                        $"{nameof(ServicePlanIdConfiguration)}.{nameof(ServicePlanIdConfiguration.ServicePlanIdRegion)} is required. " +
                        $"Set it to one of the values in {nameof(SmsServicePlanIdRegion)}, e.g. {nameof(SmsServicePlanIdRegion)}.{nameof(SmsServicePlanIdRegion.Us)}.");

                _logger?.LogInformation("Initializing SMS client with {service_plan_id} in {region}",
                    servicePlanIdConfig.ServicePlanId,
                    servicePlanIdConfig.ServicePlanIdRegion.Value);

                var smsBaseUrl = ResolveUrl(
                    _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.SmsUrl,
                    () => SinchUrlResolvers.ResolveSmsServicePlanIdUrl(sinchSmsConfiguration.ServicePlanIdConfiguration));

                var bearerSnakeHttp = new Http(new Lazy<ISinchAuth>(new BearerAuth(servicePlanIdConfig.ApiToken)),
                    _httpClientAccessor,
                    _loggerFactory?.Create<IHttp>(),
                    SnakeCaseNamingPolicy.Instance);
                return new SmsClient(new ServicePlanId(servicePlanIdConfig.ServicePlanId),
                    smsBaseUrl,
                    _loggerFactory, bearerSnakeHttp);
            }

            var unifiedCredentials = ValidateUnifiedCredentials();

            if (sinchSmsConfiguration.Region == null)
                throw new InvalidOperationException(
                    $"{nameof(SinchSmsConfiguration)}.{nameof(SinchSmsConfiguration.Region)} is required. " +
                    $"Set it to one of the values in {nameof(SmsRegion)}, e.g. {nameof(SmsRegion)}.{nameof(SmsRegion.Us)}.");

            _logger?.LogInformation("Initializing SMS client with {project_id} in {region}",
                unifiedCredentials.ProjectId,
                sinchSmsConfiguration.Region);

            var smsResolvedUrl = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.SmsUrl,
                () => SinchUrlResolvers.ResolveSmsUrl(sinchSmsConfiguration));

            return new SmsClient(
                new ProjectId(unifiedCredentials.ProjectId),
                smsResolvedUrl,
                _loggerFactory,
                _httpSnakeCase.Value);
        }

        private IHttp InitHttpSnakeCase()
        {
            return new Http(_sinchOauth, _httpClientAccessor,
                _loggerFactory?.Create<IHttp>(),
                SnakeCaseNamingPolicy.Instance);
        }

        private Http InitHttpCamelCase()
        {
            return new Http(_sinchOauth, _httpClientAccessor,
                _loggerFactory?.Create<IHttp>(),
                JsonNamingPolicy.CamelCase);
        }

        private SinchUnifiedCredentials ValidateUnifiedCredentials()
        {
            if (_sinchClientConfiguration.SinchUnifiedCredentials == null)
            {
                throw new ArgumentNullException($"{nameof(SinchClientConfiguration.SinchUnifiedCredentials)} is null.");
            }

            var credentials = _sinchClientConfiguration.SinchUnifiedCredentials;
            var exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(credentials.ProjectId))
                exceptions.Add(new InvalidOperationException($"{nameof(credentials.ProjectId)} should have a value"));

            if (string.IsNullOrEmpty(credentials.KeyId))
                exceptions.Add(new InvalidOperationException($"{nameof(credentials.KeyId)} should have a value"));

            if (string.IsNullOrEmpty(credentials.KeySecret))
                exceptions.Add(new InvalidOperationException($"{nameof(credentials.KeySecret)} should have a value"));

            if (exceptions.Any()) throw new AggregateException("Credentials are missing", exceptions);

            return credentials;
        }

        /// <summary>
        /// Resolves URL by preferring ApiUrlOverrides, then falling back to the configuration default.
        /// </summary>
        private static Uri ResolveUrl(string? urlOverride, Func<Uri> defaultResolver)
        {
            return !string.IsNullOrEmpty(urlOverride)
                ? new Uri(urlOverride)
                : defaultResolver();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            if (_httpClientFactory is DefaultHttpClientFactory defaultFactory)
            {
                defaultFactory.Dispose();
            }

            _disposed = true;
        }
    }
}
