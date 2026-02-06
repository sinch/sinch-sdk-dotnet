using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Sinch.Core;
using Sinch.Fax.Emails;
using Sinch.Logger;

namespace Sinch.Fax.Services
{
    /// <summary>
    ///     A fax service identifies a set of configuration values.
    ///     You can specify the service as a part of an API request or
    ///     by associating a Sinch number with a service.<br/><br/>
    ///     This can be useful if you want to point a group of numbers
    ///     to a particular incoming fax URL, or want to set the storage strategy
    ///     for some of your numbers but not all of them.
    /// </summary>
    public interface ISinchFaxServices
    {
        /// <summary>
        ///     Creates a new service that you can use to set default configuration values.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Service> Create(CreateServiceRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a service resource.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Service> Get(string serviceId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update settings on the service.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Service> Update(UpdateServiceRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete a service.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string serviceId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     List services
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListServicesResponse> List(int? page = null, int? pageSize = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Auto List Services
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<Service> ListAuto(int? page = null, int? pageSize = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///     List emails for a number.
        /// </summary>
        /// <param name="serviceId">The serviceId containing the numbers you want to list.</param>
        /// <param name="phoneNumber">The phone number you want to get emails for.</param>
        /// <param name="page">The page number to fetch. If not specified, the first page will be returned.</param>
        /// <param name="pageSize">Number of items to return on each page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An object of page with a list of email addresses</returns>
        Task<ListEmailsResponse<string>> ListEmailsForNumber(string serviceId, string phoneNumber, int? page = null,
            int? pageSize = null,
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
        IAsyncEnumerable<string> ListEmailsForNumberAuto(string serviceId, string phoneNumber, int? page = null,
            int? pageSize = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     List numbers for a service.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListNumbersResponse> ListNumbers(string serviceId, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Auto List numbers for a service.
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<ServicePhoneNumber> ListNumbersAuto(string serviceId, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default);
    }

    internal sealed class ServicesClient : ISinchFaxServices
    {
        private const int DefaultPageSize = 20;
        private const int EmailsDefaultPageSize = 100;

        private readonly string _projectId;
        private readonly ILoggerAdapter<ISinchFaxServices>? _logger;
        private readonly IHttp _http;
        private readonly Uri _apiBasePath;

        public ServicesClient(string projectId, Uri baseAddress, ILoggerAdapter<ISinchFaxServices>? logger, IHttp http)
        {
            _projectId = projectId;
            _logger = logger;
            _http = http;
            _apiBasePath = new Uri(baseAddress, $"/v3/projects/{projectId}/services");
        }

        public Task<Service> Create(CreateServiceRequest request, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Creating a service for {projectId}", _projectId);

            return _http.Send<CreateServiceRequest, Service>(_apiBasePath, HttpMethod.Post, request, cancellationToken);
        }

        public Task<Service> Get(string serviceId, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Getting a service with {id} for {projectId}", serviceId, _projectId);
            ExceptionUtils.CheckEmptyString(nameof(serviceId), serviceId);
            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += "/" + serviceId;
            return _http.Send<Service>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }

        public Task<Service> Update(UpdateServiceRequest request, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Updating a {serviceId} for {projectId}", request.Id, _projectId);
            ExceptionUtils.CheckEmptyString(nameof(request.Id), request.Id);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += "/" + request.Id;
            return _http.Send<UpdateServiceRequest, Service>(uriBuilder.Uri, HttpMethod.Patch, request,
                cancellationToken);
        }

        public Task Delete(string serviceId, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Deleting a {serviceId} from {projectId}", serviceId, _projectId);
            ExceptionUtils.CheckEmptyString(nameof(serviceId), serviceId);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += "/" + serviceId;
            return _http.Send<EmptyResponse>(uriBuilder.Uri, HttpMethod.Delete, cancellationToken);
        }

        public Task<ListServicesResponse> List(int? page = null, int? pageSize = null, CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Listing services for {projectId}", _projectId);

            var uriBuilder = new UriBuilder(_apiBasePath);
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var actualPageSize = pageSize ?? DefaultPageSize;

            if (page.HasValue)
            {
                queryString.Add("page", page.Value.ToString());
            }

            queryString.Add("pageSize", actualPageSize.ToString());

            uriBuilder.Query = queryString.ToString();

            return _http.Send<ListServicesResponse>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }

        public async IAsyncEnumerable<Service> ListAuto(int? page = null, int? pageSize = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto Listing services for {projectId}", _projectId);

            page = Utils.GetAutoPage(page, PageStart.One);

            do
            {
                var response = await List(page, pageSize, cancellationToken);
                
                foreach (var service in response.Services)
                    yield return service;

                if (Utils.IsLastPage(page.Value, response.PageSize, response.TotalItems, PageStart.One))
                {
                    yield break;
                }

                page += 1;
            } while (true);
        }

        /// <inheritdoc />
        public Task<ListEmailsResponse<string>> ListEmailsForNumber(string serviceId, string phoneNumber, int? page = null,
            int? pageSize = null,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Listing emails for {serviceId} and {number}", serviceId, phoneNumber);
            ExceptionUtils.CheckEmptyString(nameof(serviceId), serviceId);
            ExceptionUtils.CheckEmptyString(nameof(phoneNumber), phoneNumber);

            var uriBuilder = new UriBuilder(_apiBasePath);
            uriBuilder.Path += $"/{serviceId}/numbers/{phoneNumber}/emails";
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var actualPageSize = pageSize ?? EmailsDefaultPageSize;

            if (page.HasValue)
            {
                queryString.Add("page", page.Value.ToString());
            }

            queryString.Add("pageSize", actualPageSize.ToString());

            uriBuilder.Query = queryString.ToString();
            return _http.Send<ListEmailsResponse<string>>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
        }

        public async IAsyncEnumerable<string> ListEmailsForNumberAuto(string serviceId, string phoneNumber, int? page = null,
            int? pageSize = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto Listing emails for number...");

            page = Utils.GetAutoPage(page, PageStart.One);
            
            do
            {
                var response = await ListEmailsForNumber(serviceId, phoneNumber, page, pageSize, cancellationToken);
                
                foreach (var contact in response.Emails)
                    yield return contact;

                if (Utils.IsLastPage(page.Value, response.PageSize, response.TotalItems, PageStart.One))
                {
                    yield break;
                }

                page += 1;
            } while (true);
        }

        public async Task<ListNumbersResponse> ListNumbers(string serviceId, int? page = null, int? pageSize = null,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogInformation("Listing numbers for {serviceId}...", serviceId);

            ExceptionUtils.CheckEmptyString(nameof(serviceId), serviceId);

            var uriBuilder = new UriBuilder(_apiBasePath);

            uriBuilder.Path += $"/{serviceId}/numbers";

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var actualPageSize = pageSize ?? DefaultPageSize;

            if (page.HasValue)
            {
                queryString.Add("page", page.Value.ToString());
            }

            queryString.Add("pageSize", actualPageSize.ToString());

            uriBuilder.Query = queryString.ToString();
            var response =
                await _http.Send<ListServiceNumbersResponse>(uriBuilder.Uri, HttpMethod.Get, cancellationToken);
            return new ListNumbersResponse()
            {
                PhoneNumbers = response.Numbers,
                Page = response.Page,
                TotalItems = response.TotalItems,
                PageSize = response.PageSize,
                TotalPages = response.TotalPages
            };
        }

        public async IAsyncEnumerable<ServicePhoneNumber> ListNumbersAuto(string serviceId, int? page = null,
            int? pageSize = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto Listing numbers for {serviceId}", serviceId);

            ExceptionUtils.CheckEmptyString(nameof(serviceId), serviceId);

            page = Utils.GetAutoPage(page, PageStart.One);

            do
            {
                var response = await ListNumbers(serviceId, page, pageSize, cancellationToken);
                
                foreach (var number in response.PhoneNumbers)
                    yield return number;

                if (Utils.IsLastPage(page.Value, response.PageSize, response.TotalItems, PageStart.One))
                {
                    yield break;
                }

                page += 1;
            } while (true);
        }
    }
}
