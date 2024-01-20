using Sinch.Core;
using Sinch.Logger;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sinch.Faxes
{
    public partial class Faxes
    {
        private readonly string _projectId;
        private readonly Uri _uri;

        private readonly Http _http;
        private ILoggerAdapter<Faxes> _loggerAdapter;
        private FileExtensionContentTypeProvider _mimeMapper;


        internal Faxes(string projectId, Uri uri, ILoggerAdapter<Faxes> loggerAdapter, Http httpClient)
        {
            this._projectId = projectId;
            this._uri = uri;
            this._loggerAdapter = loggerAdapter;
            this._http = httpClient;
            _mimeMapper = new FileExtensionContentTypeProvider();
            uri = new Uri(uri, $"/v3/projects/{projectId}/faxes");
        }
        /// <summary>
        /// Send a fax
        /// </summary>
        /// <param name="to">Number to send to</param>
        /// <param name="filePath">Path to file to fax</param>
        /// <param name="from">Sinch number you want to set as from </param>
        /// /// <param name="contentUrls">content Urls to fax</param>
        /// <param name="callbackUrl">Callback url to notify when fax is completed or failed</param>
        public async Task<Fax> Send(string to, string filePath, string from = "", string CallbackUrl = null, string[] contentUrl = null)
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

            /* Unmerged change from project 'Sinch (net6.0)'
            Before:
                    }





                    public async Task<Fax> Send(Fax fax, Stream fileContent, string fileName)
            After:
                    }





                    public async Task<Fax> Send(Fax fax, Stream fileContent, string fileName)
            */

            /* Unmerged change from project 'Sinch (net8.0)'
            Before:
                    }





                    public async Task<Fax> Send(Fax fax, Stream fileContent, string fileName)
            After:
                    }





                    public async Task<Fax> Send(Fax fax, Stream fileContent, string fileName)
            */
        }





        public async Task<Fax> Send(Fax fax, Stream fileContent, string fileName)
        {

            Fax result = await _http.SendMultipart<Fax, Fax>(_uri, fax, fileContent, fileName);
            return result;
        }

        public Fax List()
        {
            throw new NotImplementedException();
        }
        public Fax List(ListOptions listOptions)
        {
            throw new NotImplementedException();
        }
        public Fax Get(string faxId)
        {
            throw new NotImplementedException();
        }
    }
}
