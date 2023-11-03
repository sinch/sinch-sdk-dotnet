using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Numbers.Active;
using Sinch.Numbers.Available;
using Sinch.Numbers.Available.List;
using Sinch.Numbers.Available.Rent;
using Sinch.Numbers.Available.RentAny;

namespace Sinch.Numbers
{
    public interface ISinchNumbersAvailable
    {
        /// <summary>
        ///     Search for and activate an available Sinch virtual number all in one API call.
        ///     Currently the rentAny operation works only for US 10DLC numbers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ActiveNumber> RentAny(RentAnyNumberRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Activate a virtual number to use with SMS products, Voice products, or both. <br /><br />
        ///     You'll use SmsConfiguration to setup your number for SMS and VoiceConfiguration for Voice.
        ///     To setup for both, add both objects. <br /><br />
        ///     Note: You cannot add both objects if you only need to configure one object.
        ///     For example, if you only need to configure smsConfiguration for SMS messaging,
        ///     do not add the voiceConfiguration object or it will result in an error.
        /// </summary>
        /// <param name="phoneNumber">Output only. The phone number in E.164 format with leading +.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ActiveNumber> Rent(string phoneNumber,
            RentActiveNumberRequest request, CancellationToken cancellationToken = default);


        /// <summary>
        ///     Search for virtual numbers that are available for you to activate.
        ///     You can filter by any property on the available number resource. <br /><br />
        ///     When searching, indicate the capability of the number in the array as SMS and/or VOICE.
        ///     To search for a number capable of both, list both SMS and VOICE.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListAvailableNumbersResponse> List(
            ListAvailableNumbersRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Allows you to enter a specific phone number to check if it's available for use.
        /// </summary>
        /// <param name="phoneNumber">
        ///     Output only. The phone number in <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see>
        ///     format with leading +.
        ///     <example>+12025550134</example>
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AvailableNumber> CheckAvailability(string phoneNumber,
            CancellationToken cancellationToken = default);
    }

    internal class AvailableNumbers : ISinchNumbersAvailable
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<AvailableNumbers> _logger;
        private readonly string _projectId;

        public AvailableNumbers(string projectId, Uri baseAddress,
            ILoggerAdapter<AvailableNumbers> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<ActiveNumber> RentAny(RentAnyNumberRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Trying to rent any number");

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/availableNumbers:rentAny");

            return _http.Send<RentAnyNumberRequest, ActiveNumber>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Rent(string phoneNumber, RentActiveNumberRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Renting a {number}", phoneNumber);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/availableNumbers/{phoneNumber}:rent");
            return _http.Send<RentActiveNumberRequest, ActiveNumber>(uri, HttpMethod.Post, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListAvailableNumbersResponse> List(ListAvailableNumbersRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Listing available numbers");

            var pathAndQuery = $"v1/projects/{_projectId}/availableNumbers?{request.GetQueryString()}";
            var uri = new Uri(_baseAddress, pathAndQuery);
            return _http.Send<ListAvailableNumbersResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<AvailableNumber> CheckAvailability(string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Checking {number} availability", phoneNumber);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/availableNumbers/{phoneNumber}");

            var result = await _http.Send<AvailableNumber>(uri, HttpMethod.Get, cancellationToken);

            _logger?.LogDebug("Finished checking {number} availability", phoneNumber);

            return result;
        }
    }
}
