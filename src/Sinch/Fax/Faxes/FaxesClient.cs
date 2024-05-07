using System;
using System.IO;
using System.Net.Http;
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
    }

    internal sealed class FaxesClient : ISinchFaxFaxes
    {
        private readonly string _projectId;
        private readonly Uri _uri;

        private readonly IHttp _http;
        private ILoggerAdapter<ISinchFaxFaxes>? _loggerAdapter;
        private FileExtensionContentTypeProvider _mimeMapper;

        internal FaxesClient(string projectId, Uri uri, ILoggerAdapter<ISinchFaxFaxes>? loggerAdapter, IHttp httpClient)
        {
            _projectId = projectId;

            _loggerAdapter = loggerAdapter;
            _http = httpClient;
            _mimeMapper = new FileExtensionContentTypeProvider();
            uri = new Uri(uri, $"/v3/projects/{projectId}/faxes");
            _uri = uri;
        }

        /// <summary>
        /// Send a fax
        /// </summary>
        /// <param name="to">Number to send to</param>
        /// <param name="filePath">Path to file to fax</param>
        /// <param name="from">Sinch number you want to set as from </param>
        /// <param name="callbackUrl"></param>
        /// <param name="contentUrl">Callback url to notify when fax is completed or failed</param>
        public async Task<Fax> Send(string to, string filePath, string? from = null, string? callbackUrl = null,
            string[]? contentUrl = null)
        {
            var fileContent = File.OpenRead(filePath);
            var fileName = Path.GetFileName(filePath);
            var fax = new Fax
            {
                To = to,
                From = from,
                CallbackUrl = callbackUrl,
                ContentUrl = contentUrl
            };
            return await Send(fax, fileContent, fileName);
        }


        public async Task<Fax> Send(Fax fax, Stream fileContent, string fileName)
        {
            Fax result = await _http.SendMultipart<Fax, Fax>(_uri, fax, fileContent, fileName);
            return result;
        }

        public async Task<ListOfFaxes> List()
        {
            return await List(new ListOptions());
        }

        public async Task<ListOfFaxes> List(ListOptions listOptions)
        {
            var uribuilder = new UriBuilder(_uri.ToString());
            uribuilder.Query = listOptions.ToQueryString();

            return await _http.Send<ListOfFaxes>(uribuilder.Uri, HttpMethod.Get);
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
