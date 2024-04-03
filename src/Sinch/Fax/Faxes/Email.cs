using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Fax.Faxes
{
    public interface ISinchFaxEmails
    {
    }

    public class Emails : ISinchFaxEmails
    {
        private readonly string _projectId;
        private readonly Uri _uri;

        private readonly Http _http;
        private ILoggerAdapter<ISinchFaxEmails> _loggerAdapter;
        private FileExtensionContentTypeProvider _mimeMapper;


        internal Emails(string projectId, Uri uri, ILoggerAdapter<ISinchFaxEmails> loggerAdapter, Http httpClient)
        {
            this._projectId = projectId;

            this._loggerAdapter = loggerAdapter;
            this._http = httpClient;
            _mimeMapper = new FileExtensionContentTypeProvider();
            uri = new Uri(uri, $"/v3/projects/{projectId}/emails");
            this._uri = uri;
        }

        public async Task<EmailAdress> ListEmails(string email)
        {
            var result = await _http.Send<EmailAdress>(_uri, HttpMethod.Get);
            return result;
        }

        public async Task<EmailAdress> Uppdate(EmailAdress email)
        {
            var url = new Uri(_uri, $"/{email.Email}");
            var result = await _http.Send<EmailAdress, EmailAdress>(url, HttpMethod.Put, email);
            return result;
        }

        public async Task<EmailAdress> Add(EmailAdress email)
        {
            var result = await _http.Send<EmailAdress, EmailAdress>(_uri, HttpMethod.Post, email);
            return result;
        }

        public async Task<EmailAdress> Delete(EmailAdress email)
        {
            var url = new Uri(_uri, $"/{email.Email}");
            var result = await _http.Send<EmailAdress, EmailAdress>(url, HttpMethod.Delete, email);
            return result;
        }
    }

    /// <summary>
    /// Object from emails/ endoint that is used to send and recieve a fax via email
    /// </summary>
    public class EmailAdress
    {
        public string Email { get; set; }
        public List<String> PhoneNumbers { get; set; }
    }
}
