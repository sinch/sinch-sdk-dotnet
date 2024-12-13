using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
    }

    internal class TemplatesV1 : ISinchConversationTemplatesV1
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

        public async Task<IEnumerable<Template>> List(CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/templates");

            _logger?.LogDebug("Listing all template of {projectId}", _projectId);
            var response =
                await _http.Send<ListTemplatesResponse>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
            return response.Templates ?? new List<Template>();
        }

        public Task<Template> Create(CreateTemplateRequest request, CancellationToken cancellationToken = default)
        {
            
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/templates");

            _logger?.LogDebug("Creating a template in {projectId}", _projectId);
            return _http.Send<CreateTemplateRequest, Template>(uri, HttpMethod.Post, request,
                cancellationToken: cancellationToken);
        }

        private class ListTemplatesResponse
        {
            public List<Template>? Templates { get; set; }
        }
        
        
    }
}
