using System;
using System.Collections.Generic;
using System.IO;
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
        ///     Create and send a fax or multiple faxes.<br/><br/>
        ///     Fax content may be supplied via one or more files or URLs of supported filetypes.<br/><br/>
        ///     If you supply a callbackUrl the callback will be sent as multipart/form-data with the content
        ///     of the fax as an attachment to the body, unless you specify callbackUrlContentType as application/json.
        /// </summary>
        /// <param name="request">The fax request containing the recipients (To), content, and options. To can be a single phone number or a list of phone numbers in E.164 format.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of fax objects. Single recipient requests return one element; multiple recipients return multiple elements.</returns>
        public Task<List<Fax>> Send(SendFaxRequest request, CancellationToken cancellationToken = default);

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

        // the fax will return a PLAIN fax if there is ONE TO number, but an array if there  is > 1 
        private sealed class SendFaxResponse
        {
            [JsonPropertyName("faxes")]
            public List<Fax> Faxes { get; set; } = new();
        }

        /// <inheritdoc />
        public async Task<List<Fax>> Send(SendFaxRequest request, CancellationToken cancellationToken = default)
        {
            ApplyRequestDefaults(request);
            
            // Determine if we should send as JSON (base64 files) or multipart (file content or contentUrl)
            // Priority: base64 files → multipart (file content or contentUrl) → JSON
            var hasBase64Files = request.Files is not null && request.Files.Count() > 0;
            
            if (hasBase64Files)
            {
                // Base64 files are sent as JSON
                _loggerAdapter?.LogInformation("Sending fax with base64 files...");
                
                if (request.To?.Count() > 1)
                {
                    var faxResponseList = await _http.Send<SendFaxRequest, SendFaxResponse>(_uri, HttpMethod.Post,
                        request, cancellationToken: cancellationToken);
                    return faxResponseList.Faxes;
                }

                var faxJson = await _http.Send<SendFaxRequest, Fax>(_uri, HttpMethod.Post,
                    request, cancellationToken: cancellationToken);
                
                return [faxJson];
            }
            
            // Check if we have file content or content URLs - send as multipart
            var hasFileContent = request.FileContent is not null;
            var hasContentUrl = request.ContentUrl is not null && request.ContentUrl.Count > 0;
            
            if (hasFileContent || hasContentUrl)
            {
                _loggerAdapter?.LogInformation("Sending fax with file content or content URLs...");
                if (request.To?.Count() > 1)
                {
                    var faxResponse = await _http.SendMultipart<SendFaxRequest, SendFaxResponse>(_uri, request,
                        request.FileContent,
                        request.FileName ?? "file", cancellationToken: cancellationToken);
                    return faxResponse.Faxes;
                }

                var fax = await _http.SendMultipart<SendFaxRequest, Fax>(_uri, request, request.FileContent,
                    request.FileName ?? "file", cancellationToken: cancellationToken);
                
                return [fax];
            }

            // Fallback to JSON for any other case
            _loggerAdapter?.LogInformation("Sending fax with JSON...");
            
            if (request.To?.Count() > 1)
            {
                var faxResponseList = await _http.Send<SendFaxRequest, SendFaxResponse>(_uri, HttpMethod.Post,
                    request, cancellationToken: cancellationToken);
                return faxResponseList.Faxes;
            }

            var faxJsonFallback = await _http.Send<SendFaxRequest, Fax>(_uri, HttpMethod.Post,
                request, cancellationToken: cancellationToken);
            
            return [faxJsonFallback];
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
            
            ListFaxResponse response = new();
            do
            {
                response = await List(listFaxesRequest, cancellationToken);

                if (response.Faxes != null)
                {
                    foreach (var contact in response.Faxes)
                        yield return contact;
                }

                listFaxesRequest.Page = response.Page + 1;
            }
            while (Utils.IsNotLastPage(response.Page, response.PageSize, response.TotalItems, PageStart.One));
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
        
        private static void ApplyRequestDefaults(SendFaxRequest request)
        {
            request.HeaderText ??= string.Empty;
            request.HeaderPageNumbers ??= true;
            request.HeaderTimeZone ??= "America/New_York";
            request.RetryDelaySeconds ??= 60;
            request.CallbackUrlContentType ??= CallbackUrlContentType.MultipartFormData;
            request.ImageConversionMethod ??= ImageConversionMethod.Halftone;
        }
    }
}
