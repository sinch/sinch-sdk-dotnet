using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.TemplatesV2
{
    /// <summary>
    ///     The Template Management API offers a way to manage templates that can be used together with the Conversation API.
    ///     Note that you may also use the Message Composer tool on the [Sinch Customer
    ///     Dashboard](https://dashboard.sinch.com/convapi/message-composer) to [manage
    ///     templates](https://community.sinch.com/t5/Conversation-API/How-do-I-use-Message-Composer-to-create-omni-channel-message/ta-p/9890).
    ///     One can view a template as a pre-defined message that can optionally contain some parameters to facilitate some
    ///     customization of the pre-defined message. This feature can, for instance, be used to construct a generic customer
    ///     welcome message where the customer's name can be injected via a parameter. It's also possible to provide
    ///     translations to different languages when creating a template to make it possible to reuse one template for
    ///     different languages.
    /// </summary>
    public interface ISinchConversationTemplatesV2
    {
        /// <summary>
        ///     Get a template
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Template> Get(string templateId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     List all templates
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Template>> List(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Creates a template
        /// </summary>
        /// <param name="template">Template to create</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Template> Create(CreateTemplateRequest template, CancellationToken cancellationToken = default);

        /// <summary>
        ///    
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="languageCode">Optional. The translation's language code.</param>
        /// <param name="translationVersion">Optional. The translation's version.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TemplateTranslation>> ListTranslations(string templateId, string languageCode,
            string translationVersion,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update the template
        /// </summary>
        /// <param name="template"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Template> Update(UpdateTemplateRequest template, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes a template
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string templateId, CancellationToken cancellationToken = default);
    }

    internal sealed class TemplatesV2 : ISinchConversationTemplatesV2
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationTemplatesV2>? _logger;
        private readonly string _projectId;

        public TemplatesV2(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationTemplatesV2>? logger,
            IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _http = http;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task<Template> Get(string templateId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(templateId))
            {
                throw new ArgumentNullException(nameof(templateId));
            }

            var uri = new Uri(_baseAddress, $"v2/projects/{_projectId}/templates/{templateId}");

            _logger?.LogDebug($"Getting a template with {templateId}...", templateId);
            return _http.Send<Template>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Template>> List(CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v2/projects/{_projectId}/templates");

            _logger?.LogDebug("Listing all template of {projectId}", _projectId);
            var response =
                await _http.Send<ListTemplatesResponse>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
            return response.Templates ?? new List<Template>();
        }

        private sealed class ListTemplatesResponse
        {
            public List<Template>? Templates { get; set; }
        }

        /// <inheritdoc />
        public Task<Template> Create(CreateTemplateRequest template, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v2/projects/{_projectId}/templates");

            _logger?.LogDebug("Creating a template in {projectId}", _projectId);
            return _http.Send<CreateTemplateRequest, Template>(uri, HttpMethod.Post, template,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TemplateTranslation>> ListTranslations(string templateId, string languageCode,
            string translationVersion, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v2/projects/{_projectId}/templates/{templateId}/translations");

            var builder = new UriBuilder(uri);
            var parameters = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(languageCode))
            {
                parameters["language_code"] = languageCode;
            }

            if (!string.IsNullOrEmpty(translationVersion))
            {
                parameters["translation_version"] = translationVersion;
            }

            builder.Query = parameters.ToString()!;
            _logger?.LogDebug("Listing a translations for {templateId}", _projectId);
            var response =
                await _http.Send<ListTranslationsResponse>(builder.Uri, HttpMethod.Get,
                    cancellationToken: cancellationToken);
            return response.Translations ?? new List<TemplateTranslation>();
        }

        public Task<Template> Update(UpdateTemplateRequest template, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(template.Id))
            {
                throw new NullReferenceException($"{nameof(template.Id)} should have a value.");
            }

            var uri = new Uri(_baseAddress, $"v2/projects/{_projectId}/templates/{template.Id}");

            _logger?.LogDebug("Updating a template with {templateId} in {projectId}", template.Id, _projectId);
            return _http.Send<UpdateTemplateRequest, Template>(uri, HttpMethod.Put, template,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task Delete(string templateId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(templateId))
            {
                throw new ArgumentNullException(nameof(templateId));
            }

            var uri = new Uri(_baseAddress, $"v2/projects/{_projectId}/templates/{templateId}");

            _logger?.LogDebug("Deleting a template with {templateId} in {projectId}", templateId, _projectId);
            return _http.Send<EmptyResponse>(uri, HttpMethod.Delete, cancellationToken: cancellationToken);
        }

        private sealed class ListTranslationsResponse
        {
            public List<TemplateTranslation>? Translations { get; set; }
        }
    }
}
