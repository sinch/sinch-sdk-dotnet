using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Verification.Report.Response;

namespace Sinch.Verification
{
    public interface ISinchVerificationStatus
    {
        /// <summary>
        ///     Queries the verification result by sending the verification ID.
        ///     With this query you can get the result of a verification.
        /// </summary>
        /// <param name="id">The ID of the verification.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IVerificationReportResponse> GetById(string id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Queries the verification result by sending the verification
        ///     Identity (usually a phone number) and its method.
        ///     With this query you can get the result of a verification.
        /// </summary>
        /// <param name="endpoint">For type number use a E.164-compatible phone number.</param>
        /// <param name="method">The method of the verification.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IVerificationReportResponse> GetByIdentity(string endpoint, VerificationMethod method,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Queries the verification result by sending the verification Reference.
        ///     With this query you can get the result of a verification.
        /// </summary>
        /// <param name="reference">The custom reference of the verification.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IVerificationReportResponse> GetByReference(string reference,
            CancellationToken cancellationToken = default);
    }

    internal class SinchVerificationStatus : ISinchVerificationStatus
    {
        private readonly ILoggerAdapter<SinchVerificationStatus> _logger;
        private readonly Uri _baseAddress;
        private readonly IHttp _http;

        public SinchVerificationStatus(ILoggerAdapter<SinchVerificationStatus> logger, Uri baseAddress, IHttp http)
        {
            _http = http;
            _baseAddress = baseAddress;
            _logger = logger;
        }

        public Task<IVerificationReportResponse> GetById(string id, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/id/{id}");
            _logger?.LogDebug("Getting status of the verification by {id}", id);
            return _http.Send<IVerificationReportResponse>(uri, HttpMethod.Get,
                cancellationToken);
        }

        public Task<IVerificationReportResponse> GetByIdentity(string endpoint, VerificationMethod method,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/{method.Value}/number/{endpoint}");
            _logger?.LogDebug("Getting status of the verification by identity {endpoint} and {method}", endpoint,
                method);
            return _http.Send<IVerificationReportResponse>(uri, HttpMethod.Get,
                cancellationToken);
        }

        public Task<IVerificationReportResponse> GetByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/reference/{reference}");
            _logger?.LogDebug("Getting status of the verification by {reference}", reference);
            return _http.Send<IVerificationReportResponse>(uri, HttpMethod.Get,
                cancellationToken);
        }
    }
}
