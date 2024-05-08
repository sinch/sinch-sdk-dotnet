using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Fax.Faxes
{
    /// <summary>
    ///     The Fax API allows you to send and receive faxes.
    ///     You can send faxes to a single recipient or to multiple recipients.
    ///     You can also receive faxes and download them.
    /// </summary>
    public interface ISinchFaxFaxes
    {
        public Task<Fax> Send(CreateFaxRequest request, CancellationToken cancellationToken = default);

        Task<ListFaxResponse> List(ListFaxesRequest listFaxesRequest, CancellationToken cancellationToken = default);

        IAsyncEnumerable<Fax> ListAuto(ListFaxesRequest listFaxesRequest,
            CancellationToken cancellationToken = default);
    }

    internal sealed class FaxesClient : ISinchFaxFaxes
    {
        private readonly Uri _uri;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchFaxFaxes>? _loggerAdapter;

        internal FaxesClient(string projectId, Uri uri, ILoggerAdapter<ISinchFaxFaxes>? loggerAdapter, IHttp httpClient)
        {
            _loggerAdapter = loggerAdapter;
            _http = httpClient;
            _uri = new Uri(uri, $"/v3/projects/{projectId}/faxes");
        }

        public Task<Fax> Send(CreateFaxRequest request, CancellationToken cancellationToken = default)
        {
            if (request.FileContent is not null)
            {
                _loggerAdapter?.LogInformation("Sending a fax with file content...");
                return _http.SendMultipart<CreateFaxRequest, Fax>(_uri, request, request.FileContent,
                    request.FileName!, cancellationToken: cancellationToken);
            }

            if (request.ContentUrl?.Any() == true)
            {
                _loggerAdapter?.LogInformation("Sending a fax with content urls...");
                return _http.Send<CreateFaxRequest, Fax>(_uri, HttpMethod.Post, request,
                    cancellationToken: cancellationToken);
            }

            throw new InvalidOperationException(
                "Neither content urls or file content provided for a create fax request.");
        }

        public async Task<ListFaxResponse> List(ListFaxesRequest listFaxesRequest,
            CancellationToken cancellationToken = default)
        {
            _loggerAdapter?.LogInformation("Fetching a list of faxes");
            var uriBuilder = new UriBuilder(_uri.ToString())
            {
                Query = listFaxesRequest.ToQueryString()
            };

            return await _http.Send<ListFaxResponse>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }

        public async IAsyncEnumerable<Fax> ListAuto(ListFaxesRequest listFaxesRequest,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _loggerAdapter?.LogDebug("Auto Listing faxes");

            var response = await List(listFaxesRequest, cancellationToken);
            while (!Utils.IsLastPage(response.PageNumber, response.PageSize, response.TotalItems))
            {
                if (response.Faxes != null)
                    foreach (var contact in response.Faxes)
                        yield return contact;
                listFaxesRequest.Page = (response.PageNumber + 1).ToString();
                response = await List(listFaxesRequest, cancellationToken);
            }
        }

        public async Task<Fax> GetAsync(string faxId)
        {
            var url = new Uri(_uri, $"/{faxId}");
            return await _http.Send<Fax>(url, HttpMethod.Get);
        }

        public async Task<Stream> Download(string id)
        {
            var url = new Uri(_uri, $"/{id}.pdf");
            var result = await _http.Send<Stream>(url, HttpMethod.Get);
            return result;
        }
    }
}
