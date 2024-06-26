using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Messages.List;
using Sinch.Conversation.Messages.Message;
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
    public interface ISinchConversationMessages
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
        /// <returns><see cref="SendMessageResponse"/></returns>
        Task<SendMessageResponse> Send(SendMessageRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Retrieves a specific message by its ID.
        /// </summary>
        /// <param name="messageId">The unique ID of the message.</param>
        /// <param name="messagesSource"><see cref="MessageSource"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ConversationMessage> Get(string messageId, MessageSource? messagesSource = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation lists all messages sent or received via particular Processing Modes<br/><br/>
        ///     Setting the &#x60;messages_source&#x60; parameter to &#x60;CONVERSATION_SOURCE&#x60; allows
        ///     for querying messages in &#x60;CONVERSATION&#x60; mode, and setting it to &#x60;DISPATCH_SOURCE&#x60;
        ///     will allow for queries of messages in &#x60;DISPATCH&#x60; mode.<br/><br/>
        ///     Combining multiple parameters is supported for more detailed filtering of messages,
        ///     but some of them are not supported depending on the value specified for &#x60;messages_source&#x60;.
        ///     The description for each field will inform if that field may not be supported. <br/><br/>
        ///     The messages are ordered by their &#x60;accept_time&#x60; property in descending order,
        ///     where &#x60;accept_time&#x60; is a timestamp of when the message was enqueued by the Conversation API.
        ///     This means messages received most recently will be listed first.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListMessagesResponse> List(ListMessagesRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete a specific message by its ID. <br/><br/>
        ///     Note: Removing all messages of a conversation will not automatically delete the conversation.
        /// </summary>
        /// <param name="messageId">The unique ID of the message.</param>
        /// <param name="messagesSource"><see cref="MessageSource"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string messageId, MessageSource? messagesSource = default,
            CancellationToken cancellationToken = default);
    }

    /// <inheritdoc />
    internal class Messages : ISinchConversationMessages
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationMessages>? _logger;
        private readonly string _projectId;

        public Messages(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationMessages>? logger,
            IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _http = http;
            _logger = logger;
        }

        /// <inheritdoc/>  
        public Task<SendMessageResponse> Send(SendMessageRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/messages:send");
            _logger?.LogDebug("Sending a message...");
            return _http.Send<SendMessageRequest, SendMessageResponse>(uri, HttpMethod.Post, request,
                cancellationToken: cancellationToken);
        }

        //TODO: add simplified send text to app of recipient

        /// <inheritdoc/>  
        public Task<ConversationMessage> Get(string messageId, MessageSource? messagesSource = default,
            CancellationToken cancellationToken = default)
        {
            var param = GetMessageSourceQueryParam(messagesSource);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/messages/{messageId}{param}");

            _logger?.LogDebug("Getting a message with {messageId}...", messageId);
            return _http.Send<ConversationMessage>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>  
        public Task<ListMessagesResponse> List(ListMessagesRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Fetching list of messages {request}", request);
            var uri = new Uri(_baseAddress,
                $"v1/projects/{_projectId}/messages?{Utils.ToSnakeCaseQueryString(request)}");
            return _http.Send<ListMessagesResponse>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>  
        public Task Delete(string messageId, MessageSource? messagesSource = default,
            CancellationToken cancellationToken = default)
        {
            var param = GetMessageSourceQueryParam(messagesSource);
            _logger?.LogDebug("Deleting a message {messageId}", messageId);
            var uri = new Uri(_baseAddress,
                $"v1/projects/{_projectId}/messages/{messageId}{param}");
            return _http.Send<EmptyResponse>(uri, HttpMethod.Delete, cancellationToken: cancellationToken);
        }

        private static string GetMessageSourceQueryParam(MessageSource? messagesSource)
        {
            var param = messagesSource is null
                ? string.Empty
                : $"?messages_source={messagesSource.Value}";
            return param;
        }
    }
}
