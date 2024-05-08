using System;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        public async Task<ListOfFaxes> List()
        {
            return await List(new ListFaxesRequest());
        }

        public async Task<ListOfFaxes> List(ListFaxesRequest listFaxesRequest)
        {
            var uriBuilder = new UriBuilder(_uri.ToString());
            uriBuilder.Query = listFaxesRequest.ToQueryString();

            return await _http.Send<ListOfFaxes>(uriBuilder.Uri, HttpMethod.Get);
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
