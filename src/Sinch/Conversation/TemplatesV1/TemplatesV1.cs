using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.TemplatesV1
{
    /// <summary>
    ///     Version 1 endpoints for managing message templates. Currently maintained for existing users. Version 2 is recommended.
    /// </summary>
    public interface ISinchConversationTemplatesV1
    {
        /// <summary>
        ///     List all templates
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Template>> List(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Create a template
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Template> Create(CreateTemplateRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update the template
        /// </summary>
        /// <param name="template"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Template> Update(UpdateTemplateRequest template, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a template
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Template> Get(string templateId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes a template
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string templateId, CancellationToken cancellationToken = default);
    }

    internal sealed class TemplatesV1 : ISinchConversationTemplatesV1
    {
        private readonly string _projectId;
        private readonly Uri _baseAddress;
        private readonly ILoggerAdapter<ISinchConversationTemplatesV1>? _logger;
        private readonly IHttp _http;

        public TemplatesV1(string projectId, Uri templatesBaseAddress,
            ILoggerAdapter<ISinchConversationTemplatesV1>? logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = templatesBaseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Template>> List(CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/templates");

            _logger?.LogDebug("Listing all template of {projectId}", _projectId);
            var response =
                await _http.Send<ListTemplatesResponse>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
            return response.Templates ?? new List<Template>();
        }

        /// <inheritdoc />
        public Task<Template> Create(CreateTemplateRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/templates");

            _logger?.LogDebug("Creating a template in {projectId}", _projectId);
            return _http.Send<CreateTemplateRequest, Template>(uri, HttpMethod.Post, request,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<Template> Update(UpdateTemplateRequest template, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(template.Id))
            {
                throw new NullReferenceException($"{nameof(template.Id)} should have a value.");
            }

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/templates/{template.Id}");
            var builder = new UriBuilder(uri);
            if (template.UpdateMask?.Count > 0)
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("update_mask.paths", string.Join(",", template.UpdateMask));
                builder.Query = queryString.ToString();
            }

            _logger?.LogDebug("Updating a template with {templateId} in {projectId}", template.Id, _projectId);
            return _http.Send<UpdateTemplateRequest, Template>(uri, HttpMethod.Patch, template,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<Template> Get(string templateId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(templateId))
            {
                throw new ArgumentNullException(nameof(templateId));
            }

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/templates/{templateId}");

            _logger?.LogDebug($"Getting a template with {templateId}...", templateId);
            return _http.Send<Template>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task Delete(string templateId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(templateId))
            {
                throw new ArgumentNullException(nameof(templateId));
            }

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/templates/{templateId}");

            _logger?.LogDebug("Deleting a template with {templateId} in {projectId}", templateId, _projectId);
            return _http.Send<EmptyResponse>(uri, HttpMethod.Delete, cancellationToken: cancellationToken);
        }

        private sealed class ListTemplatesResponse
        {
            public List<Template>? Templates { get; set; }
        }
    }
}
