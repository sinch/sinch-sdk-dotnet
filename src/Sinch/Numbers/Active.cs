using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Numbers.Active;
using Sinch.Numbers.Active.List;
using Sinch.Numbers.Active.Update;

namespace Sinch.Numbers
{
    public interface ISinchNumbersActive
    {
        /// <summary>
        ///     Lists all virtual numbers for a project.<br /><br />
        ///     For additional info, see:
        ///     <see
        ///         href="https://developers.sinch.com/docs/numbers/api-reference/numbers/tag/Active-Number/#tag/Active-Number/operation/NumberService_ListActiveNumbers">
        ///         Documentation
        ///     </see>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListActiveNumbersResponse> List(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update a virtual phone number.
        ///     For example: you can move a number between different SMS services and give it a new, friendly name.
        ///     To update the name that displays for a customer, modify the displayName parameter. <br /><br />
        ///     You'll use smsConfiguration to update your SMS number and voiceConfiguration to update Voice.
        ///     To update for both, add both objects. Within these objects, you can update the service plan ID, campaign ID, and
        ///     scheduled provisioning status. You can also update the type of number, currency type and amount. Note: You cannot
        ///     add both objects if you only need to update one object. For example, if you only need to reconfigure
        ///     smsConfiguration for SMS messaging,
        ///     do not add the voiceConfiguration object or it will result in an error.
        /// </summary>
        /// <param name="phoneNumber">
        ///     Output only. The phone number in
        ///     <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see> format with leading +.
        /// </param>
        /// <param name="request">A request object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ActiveNumber> Update(string phoneNumber,
            UpdateActiveNumberRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get an information about a number
        /// </summary>
        /// <param name="phoneNumber">Number to get info about</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ActiveNumber> Get(string phoneNumber,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     With this endpoint, you can cancel your subscription for a specific virtual phone number.
        /// </summary>
        /// <param name="phoneNumber">
        ///     Output only. The phone number in <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see>
        ///     format with leading +.
        ///     <example>+12025550134</example>
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ActiveNumber> Release(
            string phoneNumber, CancellationToken cancellationToken = default);

        IAsyncEnumerable<ActiveNumber> ListAuto(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default);
    }

    internal class ActiveNumbers : ISinchNumbersActive
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ActiveNumbers>? _logger;
        private readonly string _projectId;

        public ActiveNumbers(string projectId, Uri baseAddress,
            ILoggerAdapter<ActiveNumbers>? logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        public Task<ListActiveNumbersResponse> List(ListActiveNumbersRequest request, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Fetching active numbers {request}", request);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/activeNumbers?{request.GetQueryString()}");
            return _http.Send<ListActiveNumbersResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<ActiveNumber> ListAuto(ListActiveNumbersRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Fetching active numbers {request}", request);
            do
            {
                var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/activeNumbers?{request.GetQueryString()}");
                var response = await _http.Send<ListActiveNumbersResponse>(uri, HttpMethod.Get, cancellationToken);
                request.PageToken = response.NextPageToken;
                foreach (var activeNumber in response.ActiveNumbers)
                {
                    yield return activeNumber;
                }
            } while (request.PageToken is not null);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Update(string phoneNumber, UpdateActiveNumberRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Updating a {number}", phoneNumber);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/activeNumbers/{phoneNumber}");

            return _http.Send<UpdateActiveNumberRequest, ActiveNumber>(uri, HttpMethod.Patch, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Get(string phoneNumber, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Fetching active {number}", phoneNumber);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/activeNumbers/{phoneNumber}");
            return _http.Send<ActiveNumber>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<ActiveNumber> Release(string phoneNumber, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Trying to release a {number}", phoneNumber);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/activeNumbers/{phoneNumber}:release");
            var result = await _http.Send<ActiveNumber>(uri, HttpMethod.Post, cancellationToken);
            _logger?.LogDebug("Released a {number}", phoneNumber);
            return result;
        }
    }
}
