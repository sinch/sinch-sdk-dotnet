using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<ListEmailsResponse<string>> ListForNumber(string serviceId, string phoneNumber, int? page = 1,
            int? pageSize = 1000,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     List emails
        /// </summary>
        /// <param name="page">he page number to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An object of page with a list of email addresses</returns>
        Task<ListEmailsResponse<EmailAddress>> List(int? page = 1, int? pageSize = 1000,
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
        IAsyncEnumerable<string> ListForNumberAuto(string serviceId, string phoneNumber, int? page = 1,
            int? pageSize = 1000,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Auto List emails
        /// </summary>
        /// <param name="page">The page number to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of emails addresses</returns>
        IAsyncEnumerable<EmailAddress> ListAuto(int? page = 1, int? pageSize = 1000,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Add an email to be used for sending and receiving faxes.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phoneNumbers">Numbers you want to associate with this email.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EmailAddress> Add(string email, string[] phoneNumbers, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete an email and associated numbers to that email to disable that email from sending and receiving faxes.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A successful task if response is 204.</returns>
        Task Delete(string email, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Set the numbers for an email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phoneNumbers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EmailAddress> Update(string email, string[] phoneNumbers, CancellationToken cancellationToken = default);

        /// <summary>
        ///     List configured numbers for an email
        /// </summary>
        /// <param name="email">The email you want to list numbers for.</param>
        /// <param name="page">Optional. The page to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListNumbersResponse> ListNumbers(string email, int? page = 1, int? pageSize = 20,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Auto List configured numbers for an email
        /// </summary>
        /// <param name="email">The email you want to list numbers for.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="page">Optional. The page to fetch. If not specified, the first page will be returned.</param>
        /// <returns></returns>
        IAsyncEnumerable<ServicePhoneNumber> ListNumbersAuto(string email, int? page = 1, int? pageSize = 20,
            CancellationToken cancellationToken = default);
    }

    internal class EmailsClient : ISinchFaxEmails
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
            _apiBasePath = new Uri(baseAddress, $"/v3/projects/{projectId}/emails");
        }


        public Task<ListEmailsResponse<string>> ListForNumber(string serviceId, string phoneNumber, int? page,
            int? pageSize,
            CancellationToken cancellationToken = default)
        {
            return _services.ListEmailsForNumber(serviceId, phoneNumber, page, pageSize, cancellationToken);
        }

        public Task<ListEmailsResponse<EmailAddress>> List(int? page, int? pageSize,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Listing emails...");

            var uriBuilder = new UriBuilder(_apiBasePath);
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

        public IAsyncEnumerable<string> ListForNumberAuto(string serviceId, string phoneNumber, int? page,
            int? pageSize,
            CancellationToken cancellationToken = default)
        {
            return _services.ListEmailsForNumberAuto(serviceId, phoneNumber, page, pageSize, cancellationToken);
        }


        public async IAsyncEnumerable<EmailAddress> ListAuto(int? page, int? pageSize,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto Listing emails");

            ListEmailsResponse<EmailAddress> response;
            do
            {
                response = await List(page, pageSize, cancellationToken);
                foreach (var email in response.Emails)
                    yield return email;
                page += 1;
            } while (!Utils.IsLastPage(response.PageNumber, response.PageSize, response.TotalItems, PageStart.One));
        }

        /// <inheritdoc />
        public Task<EmailAddress> Add(string email, string[] phoneNumbers,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Adding an {email} to {projectId}", email, _projectId);

            ExceptionUtils.CheckEmptyString(nameof(email), email);

            // NOT: API allows sending an empty list of numbers, returns 200, but don't actually create an email record
            if (phoneNumbers.Length == 0)
            {
                throw new InvalidOperationException("Phone numbers list should have at least one record");
            }

            return _http.Send<AddEmailRequest, EmailAddress>(_apiBasePath, HttpMethod.Post, new AddEmailRequest()
            {
                PhoneNumbers = phoneNumbers,
                Email = email
            }, cancellationToken);
        }

        /// <inheritdoc />
        public Task Delete(string email, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Deleting an {email} for {projectId}", email, _projectId);
            ExceptionUtils.CheckEmptyString(nameof(email), email);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += $"/{email}";
            return _http.Send<EmptyResponse>(uriBuilder.Uri, HttpMethod.Delete, cancellationToken);
        }

        /// <inheritdoc />
        public Task<EmailAddress> Update(string email, string[] phoneNumbers,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Updating an {email} for {projectId}", email, _projectId);
            ExceptionUtils.CheckEmptyString(nameof(email), email);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += $"/{email}";
            return _http.Send<UpdateEmailRequest, EmailAddress>(uriBuilder.Uri, HttpMethod.Put, new UpdateEmailRequest()
            {
                PhoneNumbers = phoneNumbers.ToList()
            }, cancellationToken);
        }

        public Task<ListNumbersResponse> ListNumbers(string email, int? page, int? pageSize,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Listing numbers for {email}...", email);

            ExceptionUtils.CheckEmptyString(nameof(email), email);

            var uriBuilder = new UriBuilder(_apiBasePath);

            uriBuilder.Path += $"/{email}/numbers";

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

        public async IAsyncEnumerable<ServicePhoneNumber> ListNumbersAuto(string email, int? page, int? pageSize,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            ExceptionUtils.CheckEmptyString(nameof(email), email);

            _logger?.LogDebug("Auto Listing numbers for {email}", email);

            ListNumbersResponse response;
            do
            {
                response = await ListNumbers(email, page, pageSize, cancellationToken);
                foreach (var number in response.PhoneNumbers)
                    yield return number;
                page += 1;
            } while (!Utils.IsLastPage(response.PageNumber, response.PageSize, response.TotalItems, PageStart.One));
        }
    }
}
