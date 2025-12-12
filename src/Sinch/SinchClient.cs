using System;
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
    public interface ISinchClient
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
        private readonly Lazy<ISinchConversation> _conversation;
        private readonly HttpClient _httpClient;

        private readonly SinchClientConfiguration _sinchClientConfiguration;

        private readonly LoggerFactory? _loggerFactory;

        private readonly Lazy<ISinchNumbers> _numbers;

        private readonly Lazy<ISinchSms> _sms;
        private readonly ILoggerAdapter<ISinchClient>? _logger;

        private readonly Lazy<ISinchFax> _fax;

        private readonly Lazy<ISinchAuth> _sinchOauth;
        private readonly Lazy<IHttp> _httpSnakeCase;
        private readonly Lazy<ISinchVerificationClient> _verification;
        private readonly Lazy<ISinchVoiceClient> _voice;

        public SinchClient(SinchClientConfiguration clientConfiguration)
        {
            _sinchClientConfiguration = clientConfiguration;

            if (clientConfiguration.SinchOptions?.LoggerFactory is not null)
                _loggerFactory = new LoggerFactory(clientConfiguration.SinchOptions.LoggerFactory);
            _logger = _loggerFactory?.Create<ISinchClient>();
            _logger?.LogInformation("Initializing SinchClient...");

            _httpClient = _sinchClientConfiguration.SinchOptions?.HttpClient ?? new HttpClient();

            _sinchOauth = new Lazy<ISinchAuth>(() =>
            {
                var unifiedCredentials = ValidateUnifiedCredentials();

                var oauthBaseUrl = ResolveUrl(
                    _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.AuthUrl,
                    _sinchClientConfiguration.SinchOAuthConfiguration.ResolveUrl);

                var auth = new OAuth(unifiedCredentials.KeyId, unifiedCredentials.KeySecret, _httpClient,
                    _loggerFactory?.Create<OAuth>(),
                    oauthBaseUrl
                );
                return auth;
            }, isThreadSafe: true);

            var httpCamelCase = new Lazy<Http>(() => new Http(_sinchOauth, _httpClient,
                _loggerFactory?.Create<IHttp>(),
                JsonNamingPolicy.CamelCase), isThreadSafe: true);

            _httpSnakeCase = new Lazy<IHttp>(() => new Http(_sinchOauth, _httpClient,
                _loggerFactory?.Create<IHttp>(),
                SnakeCaseNamingPolicy.Instance), isThreadSafe: true);

            _numbers = new Lazy<ISinchNumbers>(() =>
            {
                var unifiedCredentials = ValidateUnifiedCredentials();

                var numbersBaseUrl = ResolveUrl(
                    _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.NumbersUrl,
                    _sinchClientConfiguration.NumbersConfiguration.ResolveUrl);

                return new Numbers.Numbers(unifiedCredentials.ProjectId,
                    numbersBaseUrl,
                    _loggerFactory, httpCamelCase.Value);
            }, isThreadSafe: true);

            _sms = new Lazy<ISinchSms>(() =>
                InitSms(_sinchClientConfiguration.SmsConfiguration), isThreadSafe: true);


            _conversation = new Lazy<ISinchConversation>(() =>
            {
                var conversationConfig = _sinchClientConfiguration.ConversationConfiguration;
                var conversationBaseAddress =
                    conversationConfig.ResolveConversationUrl();
                var templatesBaseAddress = conversationConfig.ResolveTemplateUrl();

                return new SinchConversationClient(
                    _sinchClientConfiguration.SinchUnifiedCredentials
                        ?.ProjectId
                    !, // unified credentials, alongside projectId, will be validated as part of lazy call to http
                       // this is needed for working of Conversation.Webhooks.ParseEvent() to be accessible, without providing
                       // SinchUnifiedCredentials, the design regarding just a static method for this is still in discussion.
                    conversationBaseAddress,
                    templatesBaseAddress,
                    _loggerFactory,
                    _httpSnakeCase);
            }, isThreadSafe: true);


            _fax = new Lazy<ISinchFax>(() =>
            {
                var validateUnifiedCredentials = ValidateUnifiedCredentials();
                var faxUrl = _sinchClientConfiguration.FaxConfiguration.ResolveUrl();
                return new FaxClient(validateUnifiedCredentials.ProjectId, faxUrl, _loggerFactory, httpCamelCase.Value);
            }, isThreadSafe: true);
            _verification = new Lazy<ISinchVerificationClient>(() =>
            {
                var config = _sinchClientConfiguration.VerificationConfiguration?.Validate();
                if (config == null)
                {
                    throw new InvalidOperationException($"{nameof(SinchVerificationConfiguration)} is not set.");
                }

                ISinchAuth auth;
                if (config.AuthStrategy == AuthStrategy.ApplicationSign)
                    auth = new ApplicationSignedAuth(config.AppKey, config.AppSecret);
                else
                    auth = new BasicAuth(config.AppKey, config.AppSecret);

                var http = new Http(new Lazy<ISinchAuth>(auth), _httpClient, _loggerFactory?.Create<IHttp>(),
                    JsonNamingPolicy.CamelCase);

                var verificationUrl = ResolveUrl(
                    _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.VerificationUrl,
                    config.ResolveUrl);

                return new SinchVerificationClient(verificationUrl, _loggerFactory, http, (auth as ApplicationSignedAuth)!);
            }, isThreadSafe: true);

            _voice = new Lazy<ISinchVoiceClient>(() =>
            {
                var config = _sinchClientConfiguration.VoiceConfiguration;

                if (config == null)
                {
                    throw new InvalidOperationException($"{nameof(SinchVoiceConfiguration)} is not set.");
                }

                config.Validate();

                ISinchAuth auth = new ApplicationSignedAuth(config.AppKey, config.AppSecret);

                var http = new Http(new Lazy<ISinchAuth>(auth), _httpClient, _loggerFactory?.Create<IHttp>(),
                    JsonNamingPolicy.CamelCase);

                var voiceUrl = ResolveUrl(
                    _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.VoiceUrl,
                    config.ResolveUrl);

                var voiceAppMgmtUrl = ResolveUrl(
                    _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.VoiceApplicationManagementUrl,
                    config.ResolveApplicationManagementUrl);

                return new SinchVoiceClient(
                    voiceUrl,
                    _loggerFactory, http, (auth as ApplicationSignedAuth)!,
                    voiceAppMgmtUrl);
            }, isThreadSafe: true);
            _logger?.LogInformation("SinchClient initialized.");
        }

        /// <inheritdoc />
        public ISinchNumbers Numbers => _numbers.Value;

        /// <inheritdoc />
        public ISinchSms Sms => _sms.Value;

        /// <inheritdoc />
        public ISinchConversation Conversation => _conversation.Value;


        /// <inheritdoc />
        public ISinchAuth Auth => _sinchOauth.Value;

        /// <inheritdoc cref="ISinchFax"/>
        public ISinchFax Fax => _fax.Value;

        /// <inheritdoc/>
        public ISinchVerificationClient Verification => _verification.Value;

        /// <inheritdoc />
        public ISinchVoiceClient Voice => _voice.Value;

        private SinchUnifiedCredentials ValidateUnifiedCredentials()
        {
            if (_sinchClientConfiguration.SinchUnifiedCredentials == null)
            {
                throw new ArgumentNullException($"{nameof(SinchClientConfiguration.SinchUnifiedCredentials)} is null.");
            }

            _sinchClientConfiguration.SinchUnifiedCredentials.Validate();
            return _sinchClientConfiguration.SinchUnifiedCredentials;
        }

        private SmsClient InitSms(SinchSmsConfiguration sinchSmsConfiguration)
        {
            if (sinchSmsConfiguration.ServicePlanIdConfiguration != null)
            {
                var servicePlanIdConfig = sinchSmsConfiguration.ServicePlanIdConfiguration;
                _logger?.LogInformation("Initializing SMS client with {service_plan_id} in {region}",
                    servicePlanIdConfig.ServicePlanId,
                    servicePlanIdConfig.ServicePlanIdRegion.Value);

                var smsBaseUrl = ResolveUrl(
                    _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.SmsUrl,
                    sinchSmsConfiguration.ServicePlanIdConfiguration.ResolveUrl);

                var bearerSnakeHttp = new Http(new Lazy<ISinchAuth>(new BearerAuth(servicePlanIdConfig.ApiToken)),
                    _httpClient,
                    _loggerFactory?.Create<IHttp>(),
                    SnakeCaseNamingPolicy.Instance);
                return new SmsClient(new ServicePlanId(servicePlanIdConfig.ServicePlanId),
                    smsBaseUrl,
                    _loggerFactory, bearerSnakeHttp);
            }

            var unifiedCredentials = ValidateUnifiedCredentials();
            _logger?.LogInformation("Initializing SMS client with {project_id} in {region}",
                unifiedCredentials.ProjectId,
                sinchSmsConfiguration.Region);

            var smsResolvedUrl = ResolveUrl(
                _sinchClientConfiguration.SinchOptions?.ApiUrlOverrides?.SmsUrl,
                sinchSmsConfiguration.ResolveUrl);

            return new SmsClient(
                new ProjectId(
                    unifiedCredentials
                        .ProjectId),
                smsResolvedUrl,
                _loggerFactory,
                _httpSnakeCase.Value);
        }

        /// <summary>
        /// Resolves URL by preferring ApiUrlOverrides, then falling back to the configuration default.
        /// </summary>
        private Uri ResolveUrl(string? urlOverride, Func<Uri> defaultResolver)
        {
            return !string.IsNullOrEmpty(urlOverride)
                ? new Uri(urlOverride)
                : defaultResolver();
        }
    }
}
