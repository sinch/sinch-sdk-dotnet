using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Voice.Applications.GetNumbers;
using Sinch.Voice.Applications.QueryNumber;
using Sinch.Voice.Applications.UnassignNumbers;
using Sinch.Voice.Applications.UpdateCallbackUrls;
using Sinch.Voice.Applications.UpdateNumbers;

namespace Sinch.Voice.Applications
{
    /// <summary>
    ///     You can use the API to manage features of applications in your project.
    /// </summary>
    public interface ISinchVoiceApplications
    {
        /// <summary>
        ///     Get information about your numbers. It returns a list of numbers that you own, as well as their capability (voice
        ///     or SMS). For the ones that are assigned to an app, it returns the application key of the app.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetNumbersResponse> GetNumbers(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Assign a number or a list of numbers to an application.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AssignNumbers(AssignNumbersRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Un-assign a number from an application.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UnassignNumber(UnassignNumberRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns any callback URLs configured for the specified application.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Callbacks> GetCallbackUrls(string applicationKey,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update the configured callback URLs for the specified application.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateCallbackUrls(UpdateCallbackUrlsRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns information about the requested number.
        /// </summary>
        /// <param name="number">The phone number you want to query.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<QueryNumberResponse> QueryNumber(string number, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc />
    internal sealed class SinchApplications : ISinchVoiceApplications
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchVoiceApplications>? _logger;

        public SinchApplications(ILoggerAdapter<ISinchVoiceApplications>? logger, Uri baseAddress, IHttp http)
        {
            _logger = logger;
            _baseAddress = baseAddress;
            _http = http;
        }

        /// <inheritdoc />
        public Task<GetNumbersResponse> GetNumbers(CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, "v1/configuration/numbers");
            _logger?.LogDebug("Getting a numbers...");
            return _http.Send<GetNumbersResponse>(uri, HttpMethod.Get,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task AssignNumbers(AssignNumbersRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, "v1/configuration/numbers");
            _logger?.LogDebug("Assigning a numbers to {applicationKey}", request.ApplicationKey);
            return _http.Send<AssignNumbersRequest, EmptyResponse>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task UnassignNumber(UnassignNumberRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, "v1/configuration/numbers");
            _logger?.LogDebug("Un-assigning a {number}", request.Number);
            return _http.Send<UnassignNumberRequest, EmptyResponse>(uri, HttpMethod.Delete, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<Callbacks> GetCallbackUrls(string applicationKey, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/configuration/callbacks/applications/{applicationKey}");
            _logger?.LogDebug("Getting callback urls...");
            return _http.Send<Callbacks>(uri, HttpMethod.Get,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task UpdateCallbackUrls(UpdateCallbackUrlsRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/configuration/callbacks/applications/{request.ApplicationKey}");
            _logger?.LogDebug("Updating callback urls...");
            return _http.Send<object, object>(uri, HttpMethod.Post, new
                {
                    url = request.Url
                },
                cancellationToken);
        }

        public Task<QueryNumberResponse> QueryNumber(string number, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/calling/query/number/{number}");
            _logger?.LogDebug("Querying a {number}", number);
            return _http.Send<QueryNumberResponse>(uri, HttpMethod.Get,
                cancellationToken);
        }
    }
}
