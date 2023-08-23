using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Messages.Send;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Messages
{
    /// <summary>
    ///     To start sending messages you must have a Conversation API
    ///     <see href="https://dashboard.sinch.com/convapi/app">app</see>.
    ///     The app holds information about the channel credentials and registered webhooks
    ///     to which the API delivers callbacks such as message delivery receipts and contact messages.
    ///     If you don't already have an app please follow the instructions in the getting started guide available
    ///     in the <see href="https://dashboard.sinch.com/convapi/getting-started">Sinch Dashboard</see>
    ///     to create one.
    /// </summary>
    public interface IMessages
    {
        /// <summary>
        ///     You can send a message from a Conversation app to a contact associated with that app.
        ///     If the recipient is not associated with an existing contact, a new contact will be created.<br/><br/>
        ///     The message is added to the active conversation with the contact if a conversation already exists.
        ///     If no active conversation exists a new one is started automatically.<br/><br/>
        ///     You can find all of your IDs and authentication credentials on the
        ///     <see href="https://dashboard.sinch.com/settings/project-management">Sinch Customer Dashboard</see>
        /// </summary>
        /// <param name="request">A request params</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns><see cref="Send.Response"/></returns>
        Task<Send.Response> Send(Send.Request request, CancellationToken cancellationToken = default);

        Task Get();

        Task Delete();

        Task List();
    }

    /// <inheritdoc />
    internal class Messages : IMessages
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<Messages> _logger;
        private readonly string _projectId;

        public Messages(string projectId, Uri baseAddress, ILoggerAdapter<Messages> logger, IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _http = http;
            _logger = logger;
        }

        /// <inheritdoc/>  
        public Task<Response> Send(Request request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/messages:send");
            _logger?.LogDebug("Sending a message...");
            return _http.Send<Request, Response>(uri, HttpMethod.Post, request, cancellationToken);
        }

        public Task Get()
        {
            throw new NotImplementedException();
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task List()
        {
            throw new NotImplementedException();
        }
    }
}
