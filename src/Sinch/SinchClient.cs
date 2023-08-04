using System;
using System.Net.Http;
using System.Text.Json;
using Sinch.Auth;
using Sinch.Core;
using Sinch.Numbers;
using Sinch.SMS;
using LoggerFactory = Sinch.Logger.LoggerFactory;

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
                new Auth.Auth(keyId, keySecret, optionsObj.HttpClient, _loggerFactory?.Create<Auth.Auth>());
            var httpCamelCase = new Http(auth, optionsObj.HttpClient, _loggerFactory?.Create<Http>(),
                JsonNamingPolicy.CamelCase);
            var httpSnakeCase = new Http(auth, optionsObj.HttpClient, _loggerFactory?.Create<Http>(),
                SnakeCaseNamingPolicy.Instance);

            Numbers = new Numbers.Numbers(projectId, new Uri("https://numbers.api.sinch.com/"),
                _loggerFactory, httpCamelCase);

            // only three regions are available for single-account model. it's eu, us and br. 
            // So, we should map other regions provided in docs to nearest server.
            // See: https://developers.sinch.com/docs/sms/api-reference/#base-url
            var smsRegion = GetSmsRegion(optionsObj.SmsRegion);
            // General SMS rest api uses service_plan_id to performs calls
            // But SDK is based on single-account model which uses project_id
            // Thus, baseAddress for sms api is using a special endpoint where service_plan_id is replaced with projectId
            // for each provided endpoint
            Sms = new Sms(projectId, new Uri($"https://zt.{smsRegion}.sms.api.sinch.com"), _loggerFactory,
                httpSnakeCase);

            Auth = auth;

            logger?.LogInformation("SinchClient initialized.");
        }

        /// <summary>
        /// For E2E tests only. Here you can override base addresses.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="authUri"></param>
        /// <param name="numbersBaseAddress"></param>
        /// <param name="smsBaseAddress"></param>
        internal SinchClient(string projectId, Uri authUri, Uri numbersBaseAddress, Uri smsBaseAddress)
        {
            var http = new HttpClient();
            var auth = new Auth.Auth(authUri, http);
            var httpCamelCase = new Http(auth, http, null,
                JsonNamingPolicy.CamelCase);
            var httpSnakeCase = new Http(auth, http,null,
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
                SmsRegion.Us or SmsRegion.Ca or SmsRegion.Br => "us",
                SmsRegion.Eu or SmsRegion.Au => "eu",
                // unreachable
                _ => throw new ArgumentOutOfRangeException(nameof(smsRegion), smsRegion, "Region is not supported")
            };
        }

        /// <summary>
        ///     You can use the Active Number API to manage numbers you own. Assign numbers to projects,
        ///     release numbers from projects, or list all numbers assigned to a project.
        /// </summary>
        public INumbers Numbers { get; }

        public ISms Sms { get; }

        public IAuth Auth { get; }
    }
}
