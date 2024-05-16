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

    public class SinchClient : ISinchClient
    {
        private const string VerificationApiUrl = "https://verification.api.sinch.com/";
        private const string NumbersApiUrl = "https://numbers.api.sinch.com/";
        private const string SmsApiUrlTemplate = "https://zt.{0}.sms.api.sinch.com";
        private const string SmsApiServicePlanIdUrlTemplate = "https://{0}.sms.api.sinch.com";
        private const string FaxApiUrl = "https://fax.api.sinch.com/";
        private const string FaxApiUrlTemplate = "https://{0}.fax.api.sinch.com/";
        private const string ConversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";

        private const string VoiceApiUrlTemplate = "https://{0}.api.sinch.com/";

        // apparently, management api for applications have a different set url
        private const string VoiceApiApplicationManagementUrl = "https://callingapi.sinch.com/";
        private const string AuthApiUrl = "https://auth.sinch.com";
        private const string TemplatesApiUrlTemplate = "https://{0}.template.api.sinch.com/";

        private readonly ApiUrlOverrides? _apiUrlOverrides;
        private readonly ISinchAuth _auth;
        private readonly ISinchConversation _conversation;
        private readonly HttpClient _httpClient;

        private readonly string? _keyId;
        private readonly string? _keySecret;
        private readonly string? _projectId;

        private readonly LoggerFactory? _loggerFactory;

        private readonly ISinchNumbers _numbers;
        private readonly ISinchSms _sms;
        private readonly ILoggerAdapter<ISinchClient>? _logger;
        private readonly ISinchFax _fax;

        /// <summary>
        ///     Initialize a new <see cref="SinchClient" />
        /// </summary>
        /// <param name="keyId">Your Sinch Account key id.</param>
        /// <param name="keySecret">Your Sinch Account key secret.</param>
        /// <param name="projectId">Your project id.</param>
        /// <param name="options">Optional. See: <see cref="SinchOptions" /></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SinchClient(string? projectId, string? keyId, string? keySecret,
            Action<SinchOptions>? options = default)
        {
            _projectId = projectId;
            _keyId = keyId;
            _keySecret = keySecret;

            var optionsObj = new SinchOptions();
            options?.Invoke(optionsObj);

            if (optionsObj.LoggerFactory is not null) _loggerFactory = new LoggerFactory(optionsObj.LoggerFactory);
            _logger = _loggerFactory?.Create<ISinchClient>();
            _logger?.LogInformation("Initializing SinchClient...");


            if (string.IsNullOrEmpty(projectId)) _logger?.LogWarning($"{nameof(projectId)} is not set!");

            if (string.IsNullOrEmpty(keyId)) _logger?.LogWarning($"{nameof(keyId)} is not set!");

            if (string.IsNullOrEmpty(keySecret)) _logger?.LogWarning($"{nameof(keySecret)} is not set!");

            _httpClient = optionsObj.HttpClient ?? new HttpClient();

            _apiUrlOverrides = optionsObj.ApiUrlOverrides;

            ISinchAuth auth =
                // exception is throw when trying to get OAuth or Oauth dependant clients if credentials are missing
                new OAuth(_keyId!, _keySecret!, _httpClient, _loggerFactory?.Create<OAuth>(),
                    new Uri(_apiUrlOverrides?.AuthUrl ?? AuthApiUrl));
            _auth = auth;
            var httpCamelCase = new Http(auth, _httpClient, _loggerFactory?.Create<IHttp>(),
                JsonNamingPolicy.CamelCase);
            var httpSnakeCaseOAuth = new Http(auth, _httpClient, _loggerFactory?.Create<IHttp>(),
                SnakeCaseNamingPolicy.Instance);

            _numbers = new Numbers.Numbers(_projectId!, new Uri(_apiUrlOverrides?.NumbersUrl ?? NumbersApiUrl),
                _loggerFactory, httpCamelCase);

            _sms = InitSms(optionsObj, httpSnakeCaseOAuth);

            var conversationBaseAddress = new Uri(_apiUrlOverrides?.ConversationUrl ??
                                                  string.Format(ConversationApiUrlTemplate,
                                                      optionsObj.ConversationRegion.Value));
            var templatesBaseAddress = new Uri(_apiUrlOverrides?.TemplatesUrl ??
                                               string.Format(TemplatesApiUrlTemplate,
                                                   optionsObj.ConversationRegion.Value));
            _conversation = new SinchConversationClient(_projectId!, conversationBaseAddress
                , templatesBaseAddress,
                _loggerFactory, httpSnakeCaseOAuth);

            var faxUrl = ResolveFaxUrl(optionsObj.FaxRegion);
            _fax = new FaxClient(projectId!, faxUrl, _loggerFactory, httpCamelCase);

            _logger?.LogInformation("SinchClient initialized.");
        }

        private Uri ResolveFaxUrl(FaxRegion? faxRegion)
        {
            if (!string.IsNullOrEmpty(_apiUrlOverrides?.FaxUrl))
            {
                return new Uri(_apiUrlOverrides.FaxUrl);
            }

            if (!string.IsNullOrEmpty(faxRegion?.Value))
            {
                return new Uri(string.Format(FaxApiUrlTemplate, faxRegion.Value));
            }

            return new Uri(FaxApiUrl);
        }

        /// <inheritdoc />
        public ISinchNumbers Numbers
        {
            get
            {
                ValidateCommonCredentials();
                return _numbers;
            }
        }

        /// <inheritdoc />
        public ISinchSms Sms
        {
            get
            {
                if (!_sms.IsUsingServicePlanId)
                    ValidateCommonCredentials();
                return _sms;
            }
        }

        /// <inheritdoc />
        public ISinchConversation Conversation
        {
            get
            {
                ValidateCommonCredentials();
                return _conversation;
            }
        }


        /// <inheritdoc />
        public ISinchAuth Auth
        {
            get
            {
                ValidateCommonCredentials();
                return _auth;
            }
        }

        /// <inheritdoc cref="ISinchFax"/>
        public ISinchFax Fax
        {
            get
            {
                ValidateCommonCredentials();
                return _fax;
            }
        }

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
            return new SinchVerificationClient(new Uri(_apiUrlOverrides?.VerificationUrl ?? VerificationApiUrl),
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
                new Uri(_apiUrlOverrides?.VoiceUrl ?? string.Format(VoiceApiUrlTemplate, voiceRegion.Value)),
                _loggerFactory, http, (auth as ApplicationSignedAuth)!,
                new Uri(_apiUrlOverrides?.VoiceUrl ?? VoiceApiApplicationManagementUrl));
        }

        private void ValidateCommonCredentials()
        {
            var exceptions = new List<Exception>();
            if (string.IsNullOrEmpty(_keyId))
                exceptions.Add(new InvalidOperationException("keyId should have a value"));

            if (string.IsNullOrEmpty(_projectId))
                exceptions.Add(new InvalidOperationException("projectId should have a value"));

            if (string.IsNullOrEmpty(_keySecret))
                exceptions.Add(new InvalidOperationException("keySecret should have a value"));

            if (exceptions.Any()) throw new AggregateException("Credentials are missing", exceptions);
        }

        private SmsClient InitSms(SinchOptions optionsObj, IHttp httpSnakeCase)
        {
            if (optionsObj.ServicePlanIdOptions != null)
            {
                _logger?.LogInformation("Initializing SMS client with {service_plan_id} in {region}",
                    optionsObj.ServicePlanIdOptions.ServicePlanId,
                    optionsObj.ServicePlanIdOptions.Region.Value);
                var bearerSnakeHttp = new Http(new BearerAuth(optionsObj.ServicePlanIdOptions.ApiToken), _httpClient,
                    _loggerFactory?.Create<IHttp>(),
                    SnakeCaseNamingPolicy.Instance);
                return new SmsClient(new ServicePlanId(optionsObj.ServicePlanIdOptions.ServicePlanId),
                    BuildServicePlanIdSmsBaseAddress(optionsObj.ServicePlanIdOptions.Region,
                        _apiUrlOverrides?.SmsUrl),
                    _loggerFactory, bearerSnakeHttp);
            }

            _logger?.LogInformation("Initializing SMS client with {project_id} in {region}", _projectId,
                optionsObj.SmsRegion.Value);

            return new SmsClient(
                new ProjectId(
                    _projectId!), // exception is throw when trying to get SMS client property if _projectId is null
                BuildSmsBaseAddress(optionsObj.SmsRegion, _apiUrlOverrides?.SmsUrl),
                _loggerFactory,
                httpSnakeCase);
        }

        private static Uri BuildServicePlanIdSmsBaseAddress(SmsServicePlanIdRegion smsServicePlanIdRegion,
            string? smsUrlOverride)
        {
            if (!string.IsNullOrEmpty(smsUrlOverride)) return new Uri(smsUrlOverride);

            return new Uri(string.Format(SmsApiServicePlanIdUrlTemplate,
                smsServicePlanIdRegion.Value.ToLowerInvariant()));
        }

        private static Uri BuildSmsBaseAddress(SmsRegion smsRegion, string? smsUrlOverride)
        {
            if (!string.IsNullOrEmpty(smsUrlOverride)) return new Uri(smsUrlOverride);

            // General SMS rest api uses service_plan_id to performs calls
            // But SDK is based on single-account model which uses project_id
            // Thus, baseAddress for sms api is using a special endpoint where service_plan_id is replaced with projectId
            // for each provided endpoint
            return new Uri(string.Format(SmsApiUrlTemplate, smsRegion.Value.ToLowerInvariant()));
        }
    }
}
