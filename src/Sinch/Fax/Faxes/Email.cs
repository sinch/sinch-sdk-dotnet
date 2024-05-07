using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
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
            _projectId = projectId;

            _loggerAdapter = loggerAdapter;
            _http = httpClient;
            _mimeMapper = new FileExtensionContentTypeProvider();
            uri = new Uri(uri, $"/v3/projects/{projectId}/emails");
            _uri = uri;
        }

        public async Task<EmailAddress> ListEmails(string email)
        {
            var result = await _http.Send<EmailAddress>(_uri, HttpMethod.Get);
            return result;
        }

        public async Task<EmailAddress> Uppdate(EmailAddress email)
        {
            var url = new Uri(_uri, $"/{email.Email}");
            var result = await _http.Send<EmailAddress, EmailAddress>(url, HttpMethod.Put, email);
            return result;
        }

        public async Task<EmailAddress> Add(EmailAddress email)
        {
            var result = await _http.Send<EmailAddress, EmailAddress>(_uri, HttpMethod.Post, email);
            return result;
        }

        public async Task<EmailAddress> Delete(EmailAddress email)
        {
            var url = new Uri(_uri, $"/{email.Email}");
            var result = await _http.Send<EmailAddress, EmailAddress>(url, HttpMethod.Delete, email);
            return result;
        }
    }

    /// <summary>
    /// Object from emails/ endoint that is used to send and recieve a fax via email
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        ///     Gets or Sets VarEmail
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }


        /// <summary>
        ///     Numbers you want to associate with this email.
        /// </summary>
        [JsonPropertyName("phoneNumbers")]
        public List<string>? PhoneNumbers { get; set; }


        /// <summary>
        ///     The &#x60;Id&#x60; of the project associated with the call.
        /// </summary>
        [JsonPropertyName("projectId")]
        public string? ProjectId { get; private set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(Email)} {{\n");
            sb.Append($"  {nameof(Email)}: ").Append(Email).Append('\n');
            sb.Append($"  {nameof(PhoneNumbers)}: ").Append(PhoneNumbers).Append('\n');
            sb.Append($"  {nameof(ProjectId)}: ").Append(ProjectId).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
