using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
        Task<Template> Get(string templateId, CancellationToken cancellationToken = default);
    }

    internal class TemplatesV2 : ISinchConversationTemplatesV2
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationTemplatesV2> _logger;
        private readonly string _projectId;

        public TemplatesV2(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationTemplatesV2> logger,
            IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _http = http;
            _logger = logger;
        }

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
    }
}
