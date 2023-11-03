using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.SMS.DeliveryReports.Get;
using Sinch.SMS.DeliveryReports.List;

namespace Sinch.SMS.DeliveryReports
{
    public interface ISinchSmsDeliveryReports
    {
        /// <summary>
        ///     Delivery reports can be retrieved even if no callback was requested.
        ///     The difference between a summary and a full report is only that the full report contains
        ///     the phone numbers in <see href="https://community.sinch.com/t5/Glossary/E-164/ta-p/7537">E.164</see>
        ///     format for each status code.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetDeliveryReportResponse> Get(GetDeliveryReportRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     A recipient delivery report contains the message status for a single recipient phone number.
        /// </summary>
        /// <param name="batchId">The batch ID you received from sending a message.</param>
        /// <param name="recipientMsisdn">Phone number for which you to want to search.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DeliveryReport> GetForNumber(string batchId, string recipientMsisdn,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a list of finished delivery reports.<br /><br />
        ///     This operation supports pagination.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListDeliveryReportsResponse> List(ListDeliveryReportsRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a list of finished delivery reports.<br /><br />
        ///     This operation is auto-paginated.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>An async enumerable of <see cref="DeliveryReport"/></returns>
        IAsyncEnumerable<DeliveryReport> ListAuto(ListDeliveryReportsRequest request, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc />
    internal class DeliveryReports : ISinchSmsDeliveryReports
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<DeliveryReports> _logger;
        private readonly string _projectId;

        internal DeliveryReports(string projectId, Uri baseAddress, ILoggerAdapter<DeliveryReports> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<GetDeliveryReportResponse> Get(GetDeliveryReportRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress,
                $"xms/v1/{_projectId}/batches/{request.BatchId}/delivery_report?{request.GetQueryString()}");
            _logger?.LogDebug("Fetching delivery report for a batch with {id}", request.BatchId);

            return _http.Send<GetDeliveryReportRequest, GetDeliveryReportResponse>(uri, HttpMethod.Get, null, cancellationToken)!;
        }

        /// <inheritdoc />
        public Task<DeliveryReport> GetForNumber(string batchId, string recipientMsisdn,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress,
                $"xms/v1/{_projectId}/batches/{batchId}/delivery_report/{recipientMsisdn}");

            _logger?.LogDebug("Fetching delivery report for a {number} of a {batchId}", recipientMsisdn, batchId);

            return _http.Send<object, DeliveryReport>(uri, HttpMethod.Get, null, cancellationToken)!;
        }

        /// <inheritdoc />
        public Task<ListDeliveryReportsResponse> List(ListDeliveryReportsRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress,
                $"xms/v1/{_projectId}/delivery_reports?{request.GetQueryString()}");

            _logger?.LogDebug("Listing delivery reports for {projectId}", _projectId);

            return _http.Send<object, ListDeliveryReportsResponse>(uri, HttpMethod.Get, null, cancellationToken);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<DeliveryReport> ListAuto(ListDeliveryReportsRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Listing delivery reports for {projectId}", _projectId);
            bool isLastPage;
            do
            {
                var uri = new Uri(_baseAddress,
                    $"xms/v1/{_projectId}/delivery_reports?{request.GetQueryString()}");


                var response = await _http.Send<ListDeliveryReportsResponse>(uri, HttpMethod.Get, cancellationToken);
                foreach (var report in response.DeliveryReports)
                {
                    yield return report;
                }

                isLastPage = Utils.IsLastPage(response.Page, response.PageSize, response.Count);
                request.Page++;
            } while (!isLastPage);
        }
    }
}
