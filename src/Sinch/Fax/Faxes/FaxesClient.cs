using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
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
        /// <summary>
        ///     Create and send a fax.<br/><br/>
        ///     Fax content may be supplied via one or more files or URLs of supported filetypes.<br/><br/>
        ///     If you supply a callbackUrl the callback will be sent as multipart/form-data with the content
        ///     of the fax as an attachment to the body, unless you specify callbackUrlContentType as application/json.
        /// </summary>
        /// <param name="to">A phone number in [E.164](https://community.sinch.com/t5/Glossary/E-164/ta-p/7537) format, including the leading &#39;+&#39;.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<Fax> Send(string to, SendFaxRequest request, CancellationToken cancellationToken = default);


        /// <summary>
        ///     Create and send a fax to multiple receivers.<br/><br/>
        ///     Fax content may be supplied via one or more files or URLs of supported filetypes.<br/><br/>
        ///     If you supply a callbackUrl the callback will be sent as multipart/form-data with the content
        ///     of the fax as an attachment to the body, unless you specify callbackUrlContentType as application/json.
        /// </summary>
        /// <param name="to">A list of phone numbers in [E.164](https://community.sinch.com/t5/Glossary/E-164/ta-p/7537) format, including the leading &#39;+&#39;.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<Fax>> Send(List<string> to, SendFaxRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     List faxes sent (OUTBOUND) or received (INBOUND), set parameters to filter the list. 
        /// </summary>
        /// <param name="listFaxesRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListFaxResponse> List(ListFaxesRequest listFaxesRequest, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Automatically List faxes sent (OUTBOUND) or received (INBOUND), set parameters to filter the list. 
        /// </summary>
        /// <param name="listFaxesRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<Fax> ListAuto(ListFaxesRequest listFaxesRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get fax information using the ID number of the fax.
        /// </summary>
        /// <param name="id">The ID of the fax.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Fax> Get(string id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete the fax content for a fax using the ID number of the fax. Please note that this only deletes the content of the fax from storage.
        /// </summary>
        /// <param name="id">The ID of the fax.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Successful task if response is 204.</returns>
        Task DeleteContent(string id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Download the fax content. Currently, supports only pdf.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ContentResult> DownloadContent(string id, CancellationToken cancellationToken = default);
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

        /// <inheritdoc />
        public async Task<Fax> Send(string to, SendFaxRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException(nameof(to), "Should have a value");
            }

            var faxes = await Send(new List<string>() { to }, request, cancellationToken);
            return faxes.First();
        }


        // the fax will return a PLAIN fax if there is ONE TO number, but an array if there  is > 1 
        private class SendFaxResponse
        {
            [JsonPropertyName("faxes")]
            public List<Fax> Faxes { get; set; } = new();
        }

        /// <inheritdoc />
        public async Task<List<Fax>> Send(List<string> to, SendFaxRequest request,
            CancellationToken cancellationToken = default)
        {
            request.SetTo(to);
            if (request.FileContent is not null)
            {
                _loggerAdapter?.LogInformation("Sending fax with file content...");
                if (request.To!.Count > 1)
                {
                    var faxResponse = await _http.SendMultipart<SendFaxRequest, SendFaxResponse>(_uri, request,
                        request.FileContent,
                        request.FileName!, cancellationToken: cancellationToken);
                    return faxResponse.Faxes;
                }

                var fax = await _http.SendMultipart<SendFaxRequest, Fax>(_uri, request, request.FileContent,
                    request.FileName!, cancellationToken: cancellationToken);
                return new List<Fax>() { fax };
            }

            var sendingContentUrls = request.ContentUrl?.Any() == true;
            var sendingBase64Files = request.Files?.Any() == true;
            if (sendingContentUrls || sendingBase64Files)
            {
                if (sendingContentUrls)
                {
                    _loggerAdapter?.LogInformation("Sending fax with content urls...");
                }

                if (sendingBase64Files)
                {
                    _loggerAdapter?.LogInformation("Sending fax with base64 files...");
                }

                if (request.To!.Count > 1)
                {
                    var faxResponse = await _http.Send<SendFaxRequest, SendFaxResponse>(_uri, HttpMethod.Post, request,
                        cancellationToken: cancellationToken);
                    return faxResponse.Faxes;
                }

                var fax = await _http.Send<SendFaxRequest, Fax>(_uri, HttpMethod.Post, request,
                    cancellationToken: cancellationToken);
                return new List<Fax>() { fax };
            }

            throw new InvalidOperationException(
                "Neither content urls or file content or base64 files provided for a create fax request.");
        }

        /// <inheritdoc />
        public async Task<ListFaxResponse> List(ListFaxesRequest listFaxesRequest,
            CancellationToken cancellationToken = default)
        {
            _loggerAdapter?.LogInformation("Fetching a list of faxes...");
            var uriBuilder = new UriBuilder(_uri)
            {
                Query = listFaxesRequest.ToQueryString()
            };

            return await _http.Send<ListFaxResponse>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<Fax> ListAuto(ListFaxesRequest listFaxesRequest,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _loggerAdapter?.LogDebug("Auto Listing faxes");

            var response = await List(listFaxesRequest, cancellationToken);
            while (!Utils.IsLastPage(response.PageNumber, response.PageSize, response.TotalItems, PageStart.One))
            {
                if (response.Faxes != null)
                    foreach (var contact in response.Faxes)
                        yield return contact;
                listFaxesRequest.Page = (response.PageNumber + 1);
                response = await List(listFaxesRequest, cancellationToken);
            }
        }

        /// <inheritdoc />
        public Task<Fax> Get(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Fax id should have a value.");
            }

            _loggerAdapter?.LogInformation("Getting the fax with {id}", id);
            var uriBuilder = new UriBuilder(_uri);
            uriBuilder.Path += "/" + id;
            return _http.Send<Fax>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public Task DeleteContent(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Fax id should have a value.");
            }

            _loggerAdapter?.LogInformation("Deleting the content of the fax with {id}", id);
            var uriBuilder = new UriBuilder(_uri);
            uriBuilder.Path += $"/{id}/file";
            return _http.Send<EmptyResponse>(uriBuilder.Uri, HttpMethod.Delete, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ContentResult> DownloadContent(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Fax id should have a value.");
            }

            _loggerAdapter?.LogInformation("Downloading the content of the fax with {id}", id);
            var uriBuilder = new UriBuilder(_uri);
            uriBuilder.Path += $"/{id}/file.pdf"; // only pdf is supported for now
            return _http.Send<ContentResult>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }
    }
}
