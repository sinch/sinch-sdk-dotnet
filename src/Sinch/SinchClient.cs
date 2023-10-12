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
        private readonly LoggerFactory _loggerFactory;

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

            optionsObj.HttpClient ??= new HttpClient();

            var logger = _loggerFactory?.Create<SinchClient>();
            logger?.LogInformation("Initializing SinchClient...");

            IAuth auth =
                new Auth.OAuth(keyId, keySecret, optionsObj.HttpClient, _loggerFactory?.Create<Auth.OAuth>());
            var httpCamelCase = new Http(auth, optionsObj.HttpClient, _loggerFactory?.Create<Http>(),
                JsonNamingPolicy.CamelCase);
            var httpSnakeCase = new Http(auth, optionsObj.HttpClient, _loggerFactory?.Create<Http>(),
                SnakeCaseNamingPolicy.Instance);

            Numbers = new Numbers.Numbers(projectId, new Uri("https://numbers.api.sinch.com/"),
                _loggerFactory, httpCamelCase);

            // only three regions are available for single-account model. it's eu and us. 
            // So, we should map other regions provided in docs to nearest server.
            // See: https://developers.sinch.com/docs/sms/api-reference/#base-url
            var smsRegion = GetSmsRegion(optionsObj.SmsRegion);
            // General SMS rest api uses service_plan_id to performs calls
            // But SDK is based on single-account model which uses project_id
            // Thus, baseAddress for sms api is using a special endpoint where service_plan_id is replaced with projectId
            // for each provided endpoint
            Sms = new Sms(projectId, new Uri($"https://zt.{smsRegion}.sms.api.sinch.com"), _loggerFactory,
                httpSnakeCase);

            Conversation = new Conversation.Conversation(projectId,
                new Uri(
                    $"https://{optionsObj.ConversationRegion.Value}.conversation.api.sinch.com/"),
                _loggerFactory, httpSnakeCase);

            Auth = auth;

            logger?.LogInformation("SinchClient initialized.");
        }

        /// <summary>
        ///     For E2E tests only. Here you can override base addresses.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="authUri"></param>
        /// <param name="numbersBaseAddress"></param>
        /// <param name="smsBaseAddress"></param>
        internal SinchClient(string projectId, Uri authUri, Uri numbersBaseAddress, Uri smsBaseAddress)
        {
            var http = new HttpClient();
            Auth = new Auth.OAuth(authUri, http);
            var httpCamelCase = new Http(Auth, http, null,
                JsonNamingPolicy.CamelCase);
            var httpSnakeCase = new Http(Auth, http, null,
                SnakeCaseNamingPolicy.Instance);
            Numbers = new Numbers.Numbers(projectId, numbersBaseAddress, null, httpCamelCase);
            Sms = new Sms(projectId, smsBaseAddress, null, httpSnakeCase);
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
            return new SinchVerificationClient();
        }
    }
}
