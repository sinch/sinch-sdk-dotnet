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
        private Http httpSnakeCase1;
        private FileExtensionContentTypeProvider mimeMapper;


        internal Faxes(string projectId, Uri uri, ILoggerAdapter<Faxes> loggerAdapter, Http httpCamelCase)
        {
            this.projectId = projectId;
            this.uri = uri;
            this.loggerAdapter = loggerAdapter;
            this.http = httpCamelCase;
            mimeMapper = new FileExtensionContentTypeProvider();
            uri = new Uri(uri, $"/v3/projects/{projectId}/faxes");
        }

        public async Task<Fax> Send(string to, string filePath, string from = "")
        {
            var fileContent = new StreamContent(File.OpenRead(filePath));
            var fileName = Path.GetFileName(filePath);
            return await Send(to, fileContent, fileName, from);
        }
        public async Task<Fax> Send(string to, StreamContent file, string fileName, string from="")
        {
            var fax = new Fax
            {
                To = to,
                From = from
            };
            return await Send(fax, file, fileName);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to">Number to send to</param>
        /// <param name="filePath">Path to file to fax</param>
        /// <param name="from">Sinch number you want to set as from </param>
        /// <param name="headerText">Header text of fax</param>
        /// <param name="headerPageNumbers">Print page number on fax default true</param>
        /// <param name="headerTimeZone">Set specific timezone</param>
        /// <param name="retryDelaySeconds">Duration between retries</param>
        /// <param name="cancelTimeoutMinutes">Cancel retries or fax transmission after x minutes</param>
        /// <param name="labels">Custom labels you can tag a fax with</param>
        /// <param name="callbackUrl">Call back url to notify when fax is completed or failed</param>
        /// <param name="callbackContentType">JSON or multipart</param>
        /// <param name="imageConversionMethod">defautl halftone and best in most scenarios</param>
        /// <param name="serviceId"></param>
        /// <param name="maxRetries"></param>
        /// <returns></returns>
        public async Task<Fax> Send(string to, string filePath, string from = "", string headerText = "", string contentUrl="", bool headerPageNumbers = true, string headerTimeZone = "", int retryDelaySeconds = 60, int cancelTimeoutMinutes = 3, Dictionary<string, string> labels = null, string callbackUrl = "", string callbackContentType = "", ImageConversionMethod imageConversionMethod = ImageConversionMethod.HALFTONE, string serviceId = "", int maxRetries = 0)
        {
            var fileContent = new StreamContent(File.OpenRead(filePath));
            var fileName = System.IO.Path.GetFileName(filePath);
            var fax = new Fax
            {
                To = to,
                From = from,
                HeaderText = headerText,
                ContentUrl = contentUrl,
                HeaderPageNumbers = headerPageNumbers,
                HeaderTimeZone = headerTimeZone,
                RetryDelaySeconds = retryDelaySeconds,
                CancelTimeoutMinutes = cancelTimeoutMinutes,
                Labels = labels,
                CallbackUrl = callbackUrl,
                CallbackContentType = callbackContentType,
                ImageConversionMethod = imageConversionMethod,
                ServiceId = serviceId,
                MaxRetries = maxRetries
            };  
            return await Send(fax, fileContent, fileName);
        }


        private MultipartFormDataContent SerializeFaxToMultipart(Fax fax)
        {
            var content = new MultipartFormDataContent();
            var props = typeof(Fax).GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                BindingFlags.DeclaredOnly);
            foreach (var prop in props)
            {
                var value = prop.GetValue(fax);
                if (value != null)
                {
                    content.Add(new StringContent(value.ToString()), prop.Name);
                }
            }
            return content;
        }
        public async Task<Fax> Send(Fax fax, StreamContent fileContent,string fileName)
        {
            var content = SerializeFaxToMultipart(fax);
            string contentType;
            mimeMapper.TryGetContentType(fileName, out contentType);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType ?? "application/octet-stream");
            content.Add(fileContent, "file", fileName);
            var result = await http.Send<Fax>(uri, HttpMethod.Post, content, default);
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
