using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Fax.Faxes
{
    public interface ISinchFaxFaxes
    {
    }

    public class ListOfFaxes
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public IEnumerable<Fax>? Faxes { get; set; }
    }

    internal sealed class FaxesClient : ISinchFaxFaxes
    {
        private readonly string _projectId;
        private readonly Uri _uri;

        private readonly IHttp _http;
        private ILoggerAdapter<ISinchFaxFaxes> _loggerAdapter;
        private FileExtensionContentTypeProvider _mimeMapper;


        internal FaxesClient(string projectId, Uri uri, ILoggerAdapter<ISinchFaxFaxes> loggerAdapter, IHttp httpClient)
        {
            this._projectId = projectId;

            this._loggerAdapter = loggerAdapter;
            this._http = httpClient;
            _mimeMapper = new FileExtensionContentTypeProvider();
            uri = new Uri(uri, $"/v3/projects/{projectId}/faxes");
            this._uri = uri;
        }

        /// <summary>
        /// Send a fax
        /// </summary>
        /// <param name="to">Number to send to</param>
        /// <param name="filePath">Path to file to fax</param>
        /// <param name="from">Sinch number you want to set as from </param>
        /// /// <param name="contentUrls">content Urls to fax</param>
        /// <param name="callbackUrl">Callback url to notify when fax is completed or failed</param>
        public async Task<Fax> Send(string to, string filePath, string from = "", string CallbackUrl = null,
            string[] contentUrl = null)
        {
            var fileContent = File.OpenRead(filePath);
            var fileName = Path.GetFileName(filePath);
            var fax = new Fax
            {
                To = to,
                From = from,
                CallbackUrl = CallbackUrl,
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
