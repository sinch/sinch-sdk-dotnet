using Sinch.Core;
using Sinch.Logger;
using Sinch.SMS.Batches.Send;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sinch.Faxes
{
    public partial class Faxes
    {
        private readonly string projectId;
        private readonly Uri uri;

        private readonly Http http;
        private ILoggerAdapter<Faxes> loggerAdapter;
        private FileExtensionContentTypeProvider mimeMapper;


        internal Faxes(string projectId, Uri uri, ILoggerAdapter<Faxes> loggerAdapter, Http httpClient)
        {
            this.projectId = projectId;
            this.uri = uri;
            this.loggerAdapter = loggerAdapter;
            this.http = httpClient;
            mimeMapper = new FileExtensionContentTypeProvider();
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
        public async Task<Fax> Send(string to, string filePath, string from = "", string CallbackUrl=null, string[] contentUrl = null)
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
            
            Fax result = await http.SendMultipart<Fax,Fax>(uri, fax, fileContent, fileName);
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
