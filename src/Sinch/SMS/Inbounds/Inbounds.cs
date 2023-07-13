﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.SMS.Inbounds.List;

namespace Sinch.SMS.Inbounds
{
    public interface IInbounds
    {
        /// <summary>
        ///     With the list operation, you can list all inbound messages that you have received.
        ///     This operation supports pagination. Inbounds are returned in reverse chronological order.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Response> List(Request request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     With the list operation, you can list all inbound messages that you have received.
        ///     This operation is auto-paginated. Inbounds are returned in reverse chronological order.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<Inbound> ListAuto(Request request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation retrieves a specific inbound message with the provided inbound ID.
        /// </summary>
        /// <param name="inboundId">The Inbound ID found when listing inbound messages</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Inbound> Get(string inboundId, CancellationToken cancellationToken = default);
    }

    public class Inbounds : IInbounds
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<Inbounds> _logger;
        private readonly string _projectId;

        internal Inbounds(string projectId, Uri baseAddress, ILoggerAdapter<Inbounds> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        public Task<Response> List(Request request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectId}/inbounds?{request.GetQueryString()}");
            _logger?.LogDebug("Listing inbounds...");
            return _http.Send<Response>(uri, HttpMethod.Get, cancellationToken);
        }

        public async IAsyncEnumerable<Inbound> ListAuto(Request request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto listing inbounds...");
            bool isLastPage;
            do
            {
                var uri = new Uri(_baseAddress, $"xms/v1/{_projectId}/inbounds?{request.GetQueryString()}");
                _logger?.LogDebug("Auto list {page}", request.Page);
                var response = await _http.Send<Response>(uri, HttpMethod.Get, cancellationToken);
                foreach (var inbound in response.Inbounds)
                {
                    yield return inbound;
                }

                isLastPage = Utils.IsLastPage(response.Page, response.PageSize, response.Count);
                request.Page++;
            } while (!isLastPage);
        }


        public Task<Inbound> Get(string inboundId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectId}/inbounds/{inboundId}");
            _logger?.LogDebug("Getting inbound with {id}", inboundId);
            return _http.Send<Inbound>(uri, HttpMethod.Get, cancellationToken);
        }
    }
}
