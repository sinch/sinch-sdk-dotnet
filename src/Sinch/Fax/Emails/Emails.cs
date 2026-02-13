using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Sinch.Core;
using Sinch.Fax.Services;
using Sinch.Logger;

namespace Sinch.Fax.Emails
{
    /// <summary>
    ///     The Emails endpoint allows you to configure the Fax to Email functionality.
    ///     Fax to Email allows you to send an email and then receive a fax on your Sinch number or
    ///     send a fax and have it sent to your email address.
    ///     The service supports sending incoming faxes to multiple email addresses
    ///     and having many numbers associated with one email address.
    /// </summary>
    public interface ISinchFaxEmails
    {
        /// <summary>
        ///     List emails for a number.
        /// </summary>
        /// <param name="serviceId">The serviceId containing the numbers you want to list.</param>
        /// <param name="phoneNumber">The phone number you want to get emails for.</param>
        /// <param name="page">The page number to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An object of page with a list of email addresses</returns>
        Task<ListEmailsResponse<string>> ListForNumber(string serviceId, string phoneNumber, int? page = null,
            int? pageSize = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     List emails
        /// </summary>
        /// <param name="serviceId">The serviceId containing the emails you want to list.</param>
        /// <param name="page">he page number to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An object of page with a list of email addresses</returns>
        Task<ListEmailsResponse<EmailAddress>> List(string serviceId, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Auto List emails for a number.
        /// </summary>
        /// <param name="serviceId">The serviceId containing the numbers you want to list.</param>
        /// <param name="phoneNumber">The phone number you want to get emails for.</param>
        /// <param name="page">The page number to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of emails addresses</returns>
        IAsyncEnumerable<string> ListForNumberAuto(string serviceId, string phoneNumber, int? page = null,
            int? pageSize = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Auto List emails
        /// </summary>
        /// <param name="serviceId">The serviceId containing the emails you want to list.</param>
        /// <param name="page">The page number to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of emails addresses</returns>
        IAsyncEnumerable<EmailAddress> ListAuto(string serviceId, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Add an email to be used for sending and receiving faxes.
        /// </summary>
        /// <param name="serviceId">The serviceId to which you want to add the email.</param>
        /// <param name="emailRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EmailAddress> Add(string serviceId, EmailRequest emailRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete an email and associated numbers to that email to disable that email from sending and receiving faxes.
        /// </summary>
        /// <param name="serviceId">The serviceId containing the email you want to work with.</param>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A successful task if response is 204.</returns>
        Task Delete(string serviceId, string email, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Set the numbers for an email.
        /// </summary>
        /// <param name="serviceId">The serviceId containing the email you want to work with.</param>
        /// <param name="email"></param>
        /// <param name="updateRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EmailAddress> Update(string serviceId, string email, UpdateEmailRequest updateRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     List configured numbers for an email
        /// </summary>
        /// <param name="serviceId">The serviceId containing the email you want to work with.</param>
        /// <param name="email">The email you want to list numbers for.</param>
        /// <param name="page">Optional. The page to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListNumbersResponse> ListNumbers(string serviceId, string email, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Auto List configured numbers for an email
        /// </summary>
        /// <param name="serviceId">The serviceId containing the email you want to work with.</param>
        /// <param name="email">The email you want to list numbers for.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="page">Optional. The page to fetch. If not specified, the first page will be returned.</param>
        /// <returns></returns>
        IAsyncEnumerable<ServicePhoneNumber> ListNumbersAuto(string serviceId, string email, int? page = null,
            int? pageSize = null,
            CancellationToken cancellationToken = default);
    }

    internal sealed class EmailsClient : ISinchFaxEmails
    {
        private readonly string _projectId;
        private readonly Uri _apiBasePath;
        private readonly IHttp _http;
        private readonly ISinchFaxServices _services;
        private readonly ILoggerAdapter<ISinchFaxEmails>? _logger;


        internal EmailsClient(string projectId, Uri baseAddress, ILoggerAdapter<ISinchFaxEmails>? loggerAdapter,
            IHttp httpClient, ISinchFaxServices services)
        {
            _logger = loggerAdapter;
            _http = httpClient;
            _services = services;
            _projectId = projectId;
            _apiBasePath = new Uri(baseAddress, $"/v3/projects/{projectId}/services");
        }


        public Task<ListEmailsResponse<string>> ListForNumber(string serviceId, string phoneNumber, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default)
        {
            return _services.ListEmailsForNumber(serviceId, phoneNumber, page, pageSize, cancellationToken);
        }

        public Task<ListEmailsResponse<EmailAddress>> List(string serviceId, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Listing emails...");

            ArgumentException.ThrowIfNullOrEmpty(serviceId);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += $"/{serviceId}/emails";

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (page.HasValue)
            {
                queryString.Add("page", page.Value.ToString());
            }

            if (pageSize.HasValue)
            {
                queryString.Add("pageSize", pageSize.Value.ToString());
            }

            uriBuilder.Query = queryString.ToString();
            return _http.Send<ListEmailsResponse<EmailAddress>>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }

        public async IAsyncEnumerable<string> ListForNumberAuto(string serviceId, string phoneNumber, int? page = null, int? pageSize = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto Listing emails for number...");

            ListEmailsResponse<string> response;
            do
            {
                response = await ListForNumber(serviceId, phoneNumber, page, pageSize, cancellationToken);

                foreach (var contact in response.Emails)
                    yield return contact;

                page = response.Page + 1;
            }
            while (Utils.IsNotLastPage(response.Page, response.PageSize, response.TotalItems, PageStart.One));
        }

        public async IAsyncEnumerable<EmailAddress> ListAuto(string serviceId, int? page = null, int? pageSize = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto Listing emails");

            ListEmailsResponse<EmailAddress> response;
            do
            {
                response = await List(serviceId, page, pageSize, cancellationToken);

                foreach (var email in response.Emails)
                    yield return email;

                page = response.Page + 1;
            } while (Utils.IsNotLastPage(response.Page, response.PageSize, response.TotalItems, PageStart.One));
        }

        /// <inheritdoc />
        public Task<EmailAddress> Add(string serviceId, EmailRequest emailRequest,
            CancellationToken cancellationToken = default)
        {
            if (emailRequest == null)
            {
                throw new ArgumentNullException(nameof(emailRequest));
            }

            _logger?.LogInformation("Adding an {email} to {projectId} for {serviceId}", emailRequest.Email,
                _projectId, serviceId);

            ArgumentException.ThrowIfNullOrEmpty(serviceId);
            ArgumentException.ThrowIfNullOrEmpty(emailRequest.Email);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += $"/{serviceId}/emails";

            return _http.Send<EmailRequest, EmailAddress>(uriBuilder.Uri, HttpMethod.Post, emailRequest,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task Delete(string serviceId, string email, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Deleting an {email} for {projectId} from {serviceId}", email, _projectId,
                serviceId);

            ArgumentException.ThrowIfNullOrEmpty(serviceId);
            ArgumentException.ThrowIfNullOrEmpty(email);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += $"/{serviceId}/emails/{email}";

            return _http.Send<EmptyResponse>(uriBuilder.Uri, HttpMethod.Delete, cancellationToken);
        }

        /// <inheritdoc />
        public Task<EmailAddress> Update(string serviceId, string email, UpdateEmailRequest updateRequest,
            CancellationToken cancellationToken = default)
        {
            if (updateRequest == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }

            _logger?.LogInformation("Updating an {email} for {projectId} in {serviceId}", email, _projectId,
                serviceId);

            ArgumentException.ThrowIfNullOrEmpty(serviceId);
            ArgumentException.ThrowIfNullOrEmpty(email);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += $"/{serviceId}/emails/{email}";

            return _http.Send<UpdateEmailRequest, EmailAddress>(uriBuilder.Uri, HttpMethod.Put, updateRequest,
                cancellationToken);
        }

        public Task<ListNumbersResponse> ListNumbers(string serviceId, string email, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Listing numbers for {email}...", email);

            ArgumentException.ThrowIfNullOrEmpty(serviceId);
            ArgumentException.ThrowIfNullOrEmpty(email);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += $"/{serviceId}/emails/{email}/numbers";

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (page.HasValue)
            {
                queryString.Add("page", page.Value.ToString());
            }

            if (pageSize.HasValue)
            {
                queryString.Add("pageSize", pageSize.Value.ToString());
            }

            uriBuilder.Query = queryString.ToString();

            return _http.Send<ListNumbersResponse>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }

        public async IAsyncEnumerable<ServicePhoneNumber> ListNumbersAuto(string serviceId, string email,
            int? page = null, int? pageSize = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(serviceId);
            ArgumentException.ThrowIfNullOrEmpty(email);

            _logger?.LogDebug("Auto Listing numbers for {email}", email);

            ListNumbersResponse response;
            do
            {
                response = await ListNumbers(serviceId, email, page, pageSize, cancellationToken);

                foreach (var number in response.PhoneNumbers)
                    yield return number;

                page = response.Page + 1;
            } while (Utils.IsNotLastPage(response.Page, response.PageSize, response.TotalItems, PageStart.One));
        }
    }
}
