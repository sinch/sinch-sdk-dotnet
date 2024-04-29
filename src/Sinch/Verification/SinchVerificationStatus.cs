using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Verification.Common;
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
        Task<ReportSmsVerificationResponse> GetSmsById(string id, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsById" />
        Task<ReportCalloutVerificationResponse>
            GetCalloutById(string id, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsById" />
        Task<ReportFlashCallVerificationResponse> GetFlashCallById(string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Queries the verification result by sending the verification
        ///     Identity (usually a phone number) and its method.
        ///     With this query you can get the result of a verification.
        /// </summary>
        /// <param name="endpoint">For type number use a E.164-compatible phone number.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportSmsVerificationResponse> GetSmsByIdentity(string endpoint,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsByIdentity" />
        Task<ReportCalloutVerificationResponse> GetCalloutByIdentity(string endpoint,
            CancellationToken cancellationToken = default);

        Task<ReportFlashCallVerificationResponse> GetFlashcallByIdentity(string endpoint,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Queries the verification result by sending the verification Reference.
        ///     With this query you can get the result of a verification.
        /// </summary>
        /// <param name="reference">The custom reference of the verification.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportSmsVerificationResponse> GetSmsByReference(string reference,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsByReference" />
        Task<ReportFlashCallVerificationResponse> GetFlashcallByReference(string reference,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsByReference" />
        Task<ReportCalloutVerificationResponse> GetCalloutByReference(string reference,
            CancellationToken cancellationToken = default);
    }

    internal class SinchVerificationStatus : ISinchVerificationStatus
    {
        private readonly ILoggerAdapter<SinchVerificationStatus>? _logger;
        private readonly Uri _baseAddress;
        private readonly IHttp _http;

        public SinchVerificationStatus(ILoggerAdapter<SinchVerificationStatus>? logger, Uri baseAddress, IHttp http)
        {
            _http = http;
            _baseAddress = baseAddress;
            _logger = logger;
        }

        private Task<IVerificationReportResponse> GetById(string id, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/id/{id}");
            _logger?.LogDebug("Getting status of the verification by {id}", id);
            return _http.Send<IVerificationReportResponse>(uri, HttpMethod.Get,
                cancellationToken: cancellationToken);
        }

        private Task<IVerificationReportResponse> GetByIdentity(string endpoint, VerificationMethod method,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/{method.Value}/number/{endpoint}");
            _logger?.LogDebug("Getting status of the verification by identity {endpoint} and {method}", endpoint,
                method);
            return _http.Send<IVerificationReportResponse>(uri, HttpMethod.Get,
                cancellationToken: cancellationToken);
        }

        private Task<IVerificationReportResponse> GetByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/reference/{reference}");
            _logger?.LogDebug("Getting status of the verification by {reference}", reference);
            return _http.Send<IVerificationReportResponse>(uri, HttpMethod.Get,
                cancellationToken: cancellationToken);
        }

        public async Task<ReportSmsVerificationResponse> GetSmsById(string id,
            CancellationToken cancellationToken = default)
        {
            var result = await GetById(id, cancellationToken);
            return (result as ReportSmsVerificationResponse)!;
        }

        public async Task<ReportCalloutVerificationResponse> GetCalloutById(string id,
            CancellationToken cancellationToken = default)
        {
            var result = await GetById(id, cancellationToken);
            return (result as ReportCalloutVerificationResponse)!;
        }

        public async Task<ReportFlashCallVerificationResponse> GetFlashCallById(string id,
            CancellationToken cancellationToken = default)
        {
            var result = await GetById(id, cancellationToken);
            return (result as ReportFlashCallVerificationResponse)!;
        }

        public async Task<ReportSmsVerificationResponse> GetSmsByIdentity(string endpoint,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByIdentity(endpoint, VerificationMethod.Sms, cancellationToken);
            return (result as ReportSmsVerificationResponse)!;
        }

        public async Task<ReportCalloutVerificationResponse> GetCalloutByIdentity(string endpoint,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByIdentity(endpoint, VerificationMethod.Callout, cancellationToken);
            return (result as ReportCalloutVerificationResponse)!;
        }

        public async Task<ReportFlashCallVerificationResponse> GetFlashcallByIdentity(string endpoint,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByIdentity(endpoint, VerificationMethod.FlashCall, cancellationToken);
            return (result as ReportFlashCallVerificationResponse)!;
        }

        public async Task<ReportSmsVerificationResponse> GetSmsByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByReference(reference, cancellationToken);
            return (result as ReportSmsVerificationResponse)!;
        }

        public async Task<ReportFlashCallVerificationResponse> GetFlashcallByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByReference(reference, cancellationToken);
            return (result as ReportFlashCallVerificationResponse)!;
        }

        public async Task<ReportCalloutVerificationResponse> GetCalloutByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByReference(reference, cancellationToken);
            return (result as ReportCalloutVerificationResponse)!;
        }
    }
}
