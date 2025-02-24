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
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="authStrategy">
        ///     Choose which authentication to use.
        ///     Defaults to Application Sign request and it's a recommended approach.
        /// </param>
        /// <returns></returns>
        public ISinchVerificationClient Verification(string appKey, string appSecret,
            AuthStrategy authStrategy = AuthStrategy.ApplicationSign);

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
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <param name="voiceRegion">See <see cref="VoiceRegion" />. Defaults to <see cref="VoiceRegion.Global" /></param>
        /// <returns></returns>
        public ISinchVoiceClient Voice(string appKey, string appSecret, VoiceRegion? voiceRegion = null);
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
        private readonly UrlResolver _urlResolver;

        private readonly Lazy<ISinchFax> _fax;
        private readonly Lazy<Http> _httpCamelCase;

        private readonly Lazy<ISinchAuth> _sinchOauth;
        private readonly Lazy<Http> _httpSnakeCase;

        public SinchClient(SinchClientConfiguration clientConfiguration)
        {
            _sinchClientConfiguration = clientConfiguration;

            if (clientConfiguration.SinchOptions?.LoggerFactory is not null)
                _loggerFactory = new LoggerFactory(clientConfiguration.SinchOptions.LoggerFactory);
            _logger = _loggerFactory?.Create<ISinchClient>();
            _logger?.LogInformation("Initializing SinchClient...");

            if (_sinchClientConfiguration.SinchCommonCredentials == null)
            {
                _logger?.LogWarning($"{nameof(_sinchClientConfiguration.SinchCommonCredentials)} is not set!");
            }

            if (string.IsNullOrEmpty(_sinchClientConfiguration.SinchCommonCredentials?.ProjectId))
                _logger?.LogWarning(
                    $"{nameof(_sinchClientConfiguration.SinchCommonCredentials.ProjectId)} is not set!");

            if (string.IsNullOrEmpty(_sinchClientConfiguration.SinchCommonCredentials?.ProjectId))
                _logger?.LogWarning(
                    $"{nameof(_sinchClientConfiguration.SinchCommonCredentials.ProjectId)} is not set!");

            if (string.IsNullOrEmpty(_sinchClientConfiguration.SinchCommonCredentials?.ProjectId))
                _logger?.LogWarning(
                    $"{nameof(_sinchClientConfiguration.SinchCommonCredentials.ProjectId)} is not set!");

            _httpClient = _sinchClientConfiguration.SinchOptions?.HttpClient ?? new HttpClient();

            _urlResolver = new UrlResolver(_sinchClientConfiguration.SinchOptions?.ApiUrlOverrides);

            _sinchOauth = new Lazy<ISinchAuth>(() =>
                {
                    var commonCredentials = ValidateCommonCredentials();
                    var auth = new OAuth(commonCredentials!.KeyId!, commonCredentials.KeySecret!, _httpClient,
                        _loggerFactory?.Create<OAuth>(),
                        _sinchClientConfiguration.OAuthConfiguration.ResolveUrl()
                    );
                    return auth;
                }
            );
            _httpCamelCase = new Lazy<Http>(() => new Http(_sinchOauth.Value, _httpClient,
                _loggerFactory?.Create<IHttp>(),
                JsonNamingPolicy.CamelCase));

            _httpSnakeCase = new Lazy<Http>(() => new Http(_sinchOauth.Value, _httpClient,
                _loggerFactory?.Create<IHttp>(),
                SnakeCaseNamingPolicy.Instance));

            _numbers = new Lazy<ISinchNumbers>(() =>
            {
                var commonCredentials = ValidateCommonCredentials();

                return new Numbers.Numbers(commonCredentials.ProjectId,
                    _sinchClientConfiguration.NumbersConfiguration.ResolveUrl(),
                    _loggerFactory, _httpCamelCase.Value);
            });

            _sms = new Lazy<ISinchSms>(() =>
                InitSms(_sinchClientConfiguration.SmsConfiguration));


            _conversation = new Lazy<ISinchConversation>(() =>
            {
                var validateCommonCredentials = ValidateCommonCredentials();
                var conversationConfig = _sinchClientConfiguration.ConversationConfiguration;
                var conversationBaseAddress =
                    conversationConfig.ResolveConversationUrl();
                var templatesBaseAddress = conversationConfig.ResolveTemplateUrl();
                return new SinchConversationClient(validateCommonCredentials.ProjectId, conversationBaseAddress
                    , templatesBaseAddress,
                    _loggerFactory, _httpSnakeCase.Value);
            });


            _fax = new Lazy<ISinchFax>(() =>
            {
                var validateCommonCredentials = ValidateCommonCredentials();
                var faxUrl = _urlResolver.ResolveFaxUrl(_sinchClientConfiguration.FaxConfiguration.Region);
                return new FaxClient(validateCommonCredentials.ProjectId, faxUrl, _loggerFactory, _httpCamelCase.Value);
            });

            _logger?.LogInformation("SinchClient initialized.");
        }

        // /// <summary>
        // ///     Initialize a new <see cref="SinchClient" />
        // /// </summary>
        // /// <param name="keyId">Your Sinch Account key id.</param>
        // /// <param name="keySecret">Your Sinch Account key secret.</param>
        // /// <param name="projectId">Your project id.</param>
        // /// <param name="options">Optional. See: <see cref="SinchOptions" /></param>
        // /// <exception cref="ArgumentNullException"></exception>
        // public SinchClient(string? projectId, string? keyId, string? keySecret,
        //     Action<SinchOptions>? options = default)
        // {
        //     var optionsObj = new SinchOptions();
        //     options?.Invoke(optionsObj);
        //
        //     if (optionsObj.LoggerFactory is not null) _loggerFactory = new LoggerFactory(optionsObj.LoggerFactory);
        //     _logger = _loggerFactory?.Create<ISinchClient>();
        //     _logger?.LogInformation("Initializing SinchClient...");
        //
        //
        //     if (string.IsNullOrEmpty(projectId)) _logger?.LogWarning($"{nameof(projectId)} is not set!");
        //
        //     if (string.IsNullOrEmpty(keyId)) _logger?.LogWarning($"{nameof(keyId)} is not set!");
        //
        //     if (string.IsNullOrEmpty(keySecret)) _logger?.LogWarning($"{nameof(keySecret)} is not set!");
        //
        //     _httpClient = optionsObj.HttpClient ?? new HttpClient();
        //
        //     _urlResolver = new UrlResolver(optionsObj.ApiUrlOverrides);
        //
        //     ISinchAuth auth =
        //         // exception is throw when trying to get OAuth or Oauth dependant clients if credentials are missing
        //         new OAuth(_keyId!, _keySecret!, _httpClient, _loggerFactory?.Create<OAuth>(),
        //             _urlResolver.ResolveAuth());
        //     _auth = auth;
        //     _httpCamelCase = new Http(auth, _httpClient, _loggerFactory?.Create<IHttp>(),
        //         JsonNamingPolicy.CamelCase);
        //     var httpSnakeCaseOAuth = new Http(auth, _httpClient, _loggerFactory?.Create<IHttp>(),
        //         SnakeCaseNamingPolicy.Instance);
        //
        //
        //     _sms = InitSms(optionsObj, httpSnakeCaseOAuth);
        //
        //     var conversationBaseAddress =
        //         _urlResolver.ResolveConversationUrl(optionsObj.ConversationRegion);
        //     var templatesBaseAddress = _urlResolver.ResolveTemplateUrl(optionsObj.ConversationRegion);
        //     _conversation = new SinchConversationClient(_projectId!, conversationBaseAddress
        //         , templatesBaseAddress,
        //         _loggerFactory, httpSnakeCaseOAuth);
        //
        //     var faxUrl = _urlResolver.ResolveFaxUrl(optionsObj.FaxRegion);
        //     _fax = new FaxClient(projectId!, faxUrl, _loggerFactory, httpCamelCase);
        //
        //     _logger?.LogInformation("SinchClient initialized.");
        // }

        private ISinchAuth CreateOAuth()
        {
            var commonCredentials = _sinchClientConfiguration.SinchCommonCredentials;
            var auth = new OAuth(commonCredentials!.KeyId!, commonCredentials.KeySecret!, _httpClient,
                _loggerFactory?.Create<OAuth>(),
                _sinchClientConfiguration.OAuthConfiguration.ResolveUrl()
            );
            return auth;
        }

        /// <inheritdoc />
        public ISinchNumbers Numbers => _numbers.Value;

        private Http InitHttpCamelCase()
        {
            var auth = CreateOAuth();
            return new Http(auth, _httpClient, _loggerFactory?.Create<IHttp>(),
                JsonNamingPolicy.CamelCase);
        }

        /// <inheritdoc />
        public ISinchSms Sms => _sms.Value;

        /// <inheritdoc />
        public ISinchConversation Conversation => _conversation.Value;


        /// <inheritdoc />
        public ISinchAuth Auth => _sinchOauth.Value;

        /// <inheritdoc cref="ISinchFax"/>
        public ISinchFax Fax => _fax.Value;

        /// <inheritdoc/>
        public ISinchVerificationClient Verification(string appKey, string appSecret,
            AuthStrategy authStrategy = AuthStrategy.ApplicationSign)
        {
            if (string.IsNullOrEmpty(appKey))
                throw new ArgumentNullException(nameof(appKey), "The value should be present");

            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException(nameof(appSecret), "The value should be present");

            ISinchAuth auth;
            if (authStrategy == AuthStrategy.ApplicationSign)
                auth = new ApplicationSignedAuth(appKey, appSecret);
            else
                auth = new BasicAuth(appKey, appSecret);

            var http = new Http(auth, _httpClient, _loggerFactory?.Create<IHttp>(), JsonNamingPolicy.CamelCase);
            return new SinchVerificationClient(_urlResolver.ResolveVerificationUrl(),
                _loggerFactory, http);
        }

        /// <inheritdoc />
        public ISinchVoiceClient Voice(string appKey, string appSecret,
            VoiceRegion? voiceRegion = default)
        {
            if (string.IsNullOrEmpty(appKey))
                throw new ArgumentNullException(nameof(appKey), "The value should be present");

            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentNullException(nameof(appSecret), "The value should be present");

            ISinchAuth auth = new ApplicationSignedAuth(appKey, appSecret);

            voiceRegion ??= VoiceRegion.Global;

            var http = new Http(auth, _httpClient, _loggerFactory?.Create<IHttp>(), JsonNamingPolicy.CamelCase);
            return new SinchVoiceClient(
                _urlResolver.ResolveVoiceUrl(voiceRegion),
                _loggerFactory, http, (auth as ApplicationSignedAuth)!,
                _urlResolver.ResolveVoiceApplicationManagementUrl());
        }

        private SinchCommonCredentials ValidateCommonCredentials()
        {
            if (_sinchClientConfiguration.SinchCommonCredentials == null)
            {
                throw new ArgumentNullException($"{nameof(SinchClientConfiguration.SinchCommonCredentials)} is null.");
            }

            _sinchClientConfiguration.SinchCommonCredentials.Validate();
            return _sinchClientConfiguration.SinchCommonCredentials;
        }

        private SmsClient InitSms(SinchSmsConfiguration sinchSmsConfiguration)
        {
            if (sinchSmsConfiguration.ServicePlanIdConfiguration != null)
            {
                var servicePlanIdConfig = sinchSmsConfiguration.ServicePlanIdConfiguration;
                _logger?.LogInformation("Initializing SMS client with {service_plan_id} in {region}",
                    servicePlanIdConfig.ServicePlanId,
                    servicePlanIdConfig.ServicePlanIdRegion.Value);
                var bearerSnakeHttp = new Http(new BearerAuth(servicePlanIdConfig.ApiToken), _httpClient,
                    _loggerFactory?.Create<IHttp>(),
                    SnakeCaseNamingPolicy.Instance);
                return new SmsClient(new ServicePlanId(servicePlanIdConfig.ServicePlanId),
                    _urlResolver.ResolveSmsServicePlanIdUrl(servicePlanIdConfig.ServicePlanIdRegion),
                    _loggerFactory, bearerSnakeHttp);
            }

            var commonCredentials = ValidateCommonCredentials();
            _logger?.LogInformation("Initializing SMS client with {project_id} in {region}",
                commonCredentials.ProjectId,
                sinchSmsConfiguration.Region);

            return new SmsClient(
                new ProjectId(
                    commonCredentials
                        .ProjectId), // exception is throw when trying to get SMS client property if _projectId is null
                sinchSmsConfiguration.ResolveUrl(),
                _loggerFactory,
                _httpSnakeCase.Value);
        }
    }
}
