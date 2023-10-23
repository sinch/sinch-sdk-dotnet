using System;
using System.Net.Http;
using System.Text.Json;
using Sinch.Auth;
using Sinch.Conversation;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Numbers;
using Sinch.SMS;
using Sinch.Verification;

namespace Sinch
{
    public interface ISinch
    {
        /// <summary>
        ///     An OAuth2.0 functionality for an SDK in case you want to fetch tokens.
        ///     <see href="https://developers.sinch.com/docs/numbers/api-reference/authentication/oauth/">
        ///         Learn more.
        ///     </see>
        /// </summary>
        public IAuth Auth { get; }

        /// <summary>
        ///     The Numbers API enables you to search for, view, and activate numbers.
        ///     It's considered a precursor to other APIs in the Sinch product family.
        ///     The numbers API can be used in tandem with any of our APIs that perform messaging or calling.
        ///     Once you have activated your numbers, you can then use the various other APIs, such as
        ///     <see href="https://developers.sinch.com/docs/sms/">
        ///         SMS
        ///     </see>
        ///     or
        ///     <see href="">
        ///         Voice
        ///     </see>
        ///     , to assign and use those numbers. <br/><br/>
        ///     <see href="https://developers.sinch.com/docs/numbers/api-reference/">
        ///         Learn more.
        ///     </see>
        /// </summary>
        public INumbers Numbers { get; }

        /// <summary>
        ///     Send and receive SMS through a single connection for timely and cost-efficient communications using
        ///     the Sinch SMS API.<br/><br/>
        ///     <see href="https://developers.sinch.com/docs/sms/getting-started/">
        ///         Learn more.
        ///     </see>
        /// </summary>
        public ISms Sms { get; }

        /// <summary>
        ///     Send and receive messages globally over SMS, RCS, WhatsApp, Viber Business,
        ///     Facebook messenger and other popular channels using the Sinch Conversation API.<br/><br/>
        ///     The Conversation API endpoint uses built-in transcoding to give you the power of conversation
        ///     across all supported channels and, if required, full control over channel specific features.<br/><br/>
        ///     <see href="https://developers.sinch.com/docs/conversation/api-reference/">Learn more.</see>
        /// </summary>
        public IConversation Conversation { get; set; }

        /// <summary>
        ///     Verify users with SMS, flash calls (missed calls), a regular call, or data verification.
        ///     This document serves as a user guide and documentation on how to use the Sinch Verification REST APIs.
        ///     <br/><br/>
        ///     The Sinch Verification API is used to verify mobile phone numbers.
        ///     It's consumed by the Sinch Verification SDK, but it can also be used by any backend or client directly.
        ///     <br/><br/>
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
        ///             if given verification attempt was originated from device with matching phone number <br/>
        ///             Note: If you want to use data verification, please contact your account manager.
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public ISinchVerificationClient Verification(string appKey, string appSecret);
    }

    public class SinchClient : ISinch
    {
        private const string VerificationApiUrl = "https://verification.api.sinch.com/";
        private const string NumbersApiUrl = "https://numbers.api.sinch.com/";
        private const string SmsApiUrlTemplate = "https://zt.{0}.sms.api.sinch.com";
        private const string ConversationApiUrlTemplate = "https://{0}.conversation.api.sinch.com/";

        private readonly LoggerFactory _loggerFactory;
        private readonly HttpClient _httpClient;
        private readonly Uri _verificationBaseAddress = null;

        /// <summary>
        ///     Initialize a new <see cref="SinchClient"/>
        /// </summary>
        /// <param name="keyId">Your Sinch Account key id.</param>
        /// <param name="keySecret">Your Sinch Account key secret.</param>
        /// <param name="projectId">Your project id.</param>
        /// <param name="options">Optional. See: <see cref="SinchOptions"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SinchClient(string keyId, string keySecret, string projectId,
            Action<SinchOptions> options = default)
        {
            if (keyId is null)
            {
                throw new ArgumentNullException(nameof(keyId), "Should have a value");
            }

            if (keySecret is null)
            {
                throw new ArgumentNullException(nameof(keySecret), "Should have a value");
            }

            if (projectId is null)
            {
                throw new ArgumentNullException(nameof(projectId), "Should have a value");
            }

            var optionsObj = new SinchOptions();
            options?.Invoke(optionsObj);

            if (optionsObj.LoggerFactory is not null) _loggerFactory = new LoggerFactory(optionsObj.LoggerFactory);

            _httpClient = optionsObj.HttpClient ?? new HttpClient();

            var logger = _loggerFactory?.Create<SinchClient>();
            logger?.LogInformation("Initializing SinchClient...");

            IAuth auth =
                new OAuth(keyId, keySecret, optionsObj.HttpClient, _loggerFactory?.Create<OAuth>());
            var httpCamelCase = new Http(auth, optionsObj.HttpClient, _loggerFactory?.Create<Http>(),
                JsonNamingPolicy.CamelCase);
            var httpSnakeCase = new Http(auth, optionsObj.HttpClient, _loggerFactory?.Create<Http>(),
                SnakeCaseNamingPolicy.Instance);

            Numbers = new Numbers.Numbers(projectId, new Uri(NumbersApiUrl),
                _loggerFactory, httpCamelCase);
            Sms = new Sms(projectId, GetSmsBaseAddress(optionsObj.SmsRegion), _loggerFactory,
                httpSnakeCase);
            Conversation = new Conversation.Conversation(projectId,
                new Uri(string.Format(ConversationApiUrlTemplate, optionsObj.ConversationRegion.Value)),
                _loggerFactory, httpSnakeCase);

            Auth = auth;

            logger?.LogInformation("SinchClient initialized.");
        }

        private static Uri GetSmsBaseAddress(SmsRegion smsRegion)
        {
            // only three regions are available for single-account model. it's eu and us. 
            // So, we should map other regions provided in docs to nearest server.
            // See: https://developers.sinch.com/docs/sms/api-reference/#base-url
            var smsRegionStr = GetSmsRegion(smsRegion);
            // General SMS rest api uses service_plan_id to performs calls
            // But SDK is based on single-account model which uses project_id
            // Thus, baseAddress for sms api is using a special endpoint where service_plan_id is replaced with projectId
            // for each provided endpoint
            return new Uri(string.Format(SmsApiUrlTemplate, smsRegionStr));
        }

        /// <summary>
        ///     For E2E tests only. Here you can override base addresses.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="authUri"></param>
        /// <param name="numbersBaseAddress"></param>
        /// <param name="smsBaseAddress"></param>
        /// <param name="verificationBaseAddress"></param>
        internal SinchClient(string projectId, Uri authUri, Uri numbersBaseAddress, Uri smsBaseAddress,
            Uri verificationBaseAddress)
        {
            _httpClient = new HttpClient();
            Auth = new OAuth(authUri, _httpClient);
            var httpCamelCase = new Http(Auth, _httpClient, null,
                JsonNamingPolicy.CamelCase);
            var httpSnakeCase = new Http(Auth, _httpClient, null,
                SnakeCaseNamingPolicy.Instance);
            Numbers = new Numbers.Numbers(projectId, numbersBaseAddress, null, httpCamelCase);
            Sms = new Sms(projectId, smsBaseAddress, null, httpSnakeCase);
            _verificationBaseAddress = verificationBaseAddress;
        }

        /// <summary>
        ///     Only two regions are available for single-account model. it's eu, us.
        ///     So, we should map other regions provided in docs to nearest server.
        ///     See: https://developers.sinch.com/docs/sms/api-reference/#base-url
        /// </summary>
        /// <param name="smsRegion"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static string GetSmsRegion(SmsRegion smsRegion)
        {
            return smsRegion switch
            {
                _ when smsRegion == SmsRegion.Us || smsRegion == SmsRegion.Ca || smsRegion == SmsRegion.Br => "us",
                _ when smsRegion == SmsRegion.Eu || smsRegion == SmsRegion.Au => "eu",
                _ => smsRegion.Value
            };
        }

        /// <inheritdoc/>       
        public INumbers Numbers { get; }

        /// <inheritdoc/>
        public ISms Sms { get; }

        /// <inheritdoc/>
        public IConversation Conversation { get; set; }


        /// <inheritdoc/>
        public IAuth Auth { get; }

        /// <inheritdoc/>
        public ISinchVerificationClient Verification(string appKey, string appSecret)
        {
            if (string.IsNullOrEmpty(appKey))
            {
                throw new ArgumentNullException(nameof(appKey), "The value should be present");
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                throw new ArgumentNullException(nameof(appSecret), "The value should be present");
            }

            var basicAuth = new BasicAuth(appKey, appSecret);
            // TODO: implement application signed authentication, create IHttp just before the request with SignedRequestAuth
            var http = new Http(basicAuth, _httpClient, _loggerFactory?.Create<Http>(), JsonNamingPolicy.CamelCase);
            return new SinchVerificationClient(appKey, appSecret,
                _verificationBaseAddress ?? new Uri(VerificationApiUrl),
                _loggerFactory, http);
        }
    }
}
