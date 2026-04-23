using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Messages.List;
using Sinch.Conversation.Messages.Message;
using Sinch.Conversation.Messages.Send;
using Sinch.Conversation.Messages.Update;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Messages
{
    /// <summary>
    ///     To start sending messages you must have a Conversation API
    ///     <see href="https://dashboard.sinch.com/convapi/app">app</see>.
    ///     The app holds information about the channel credentials and registered event destinations
    ///     to which the API delivers sinch events such as message delivery receipts and contact messages.
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
        ///     This operation lists all messages using server-default settings.
        ///     See <see cref="List(ListMessagesRequest, CancellationToken)" /> to apply filters.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="ListMessagesResponse"/> containing the first page of messages.</returns>
        Task<ListMessagesResponse> List(CancellationToken cancellationToken = default);

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

        /// <summary>
        ///     Retrieves the last message sent to specified channel identities.<br/><br/>
        ///     In <c>CONVERSATION_SOURCE</c> mode, you can query either by <c>channel_identities</c> or by <c>contact_ids</c>.<br/>
        ///     Note: Use either <c>contact_ids</c> OR <c>channel_identities</c> per request, not both.<br/>
        ///     <c>DISPATCH_SOURCE</c> mode does not support <c>contact_ids</c>.
        /// </summary>
        /// <param name="request">The filter parameters, including channel identities or contact IDs, message source, and pagination options.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="ListMessagesResponse"/> containing the matched messages and an optional next page token.</returns>
        Task<ListMessagesResponse> ListLastMessagesByChannelIdentity(ListMessagesByChannelIdentityRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     See <see cref="List(CancellationToken)" />, but lists all messages automatically.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>An async sequence of all <see cref="ConversationMessage"/> items across all pages.</returns>
        IAsyncEnumerable<ConversationMessage> ListAuto(CancellationToken cancellationToken = default);

        /// <summary>
        ///     See <see cref="List(ListMessagesRequest, CancellationToken)" />, but lists all messages automatically.
        /// </summary>
        /// <param name="request">Filters and pagination options used as the initial request; page tokens are managed automatically.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>An async sequence of <see cref="ConversationMessage"/> items across all pages.</returns>
        IAsyncEnumerable<ConversationMessage> ListAuto(ListMessagesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates the metadata of a specific message.
        /// </summary>
        /// <param name="messageId">The unique ID of the message to update.</param>
        /// <param name="metadata">The new metadata value to set on the message. Up to 1024 characters long.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>The updated <see cref="ConversationMessage"/>.</returns>
        Task<ConversationMessage> Update(string messageId, string metadata,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Automatically iterates over all pages of last messages sent to specified channel identities.<br/><br/>
        ///     In <c>CONVERSATION_SOURCE</c> mode, you can query either by <c>channel_identities</c> or by <c>contact_ids</c>.<br/>
        ///     Note: Use either <c>contact_ids</c> OR <c>channel_identities</c> per request, not both.<br/>
        ///     <c>DISPATCH_SOURCE</c> mode does not support <c>contact_ids</c>.
        /// </summary>
        /// <param name="request">The filter parameters. <see cref="ListMessagesByChannelIdentityRequest.PageToken"/> will be managed automatically.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>An async sequence of <see cref="ConversationMessage"/> items across all pages.</returns>
        IAsyncEnumerable<ConversationMessage> ListLastMessagesByChannelIdentityAuto(
            ListMessagesByChannelIdentityRequest request,
            CancellationToken cancellationToken = default);
    }

    /// <inheritdoc />
    internal sealed class Messages : ISinchConversationMessages
    {
        private readonly Uri _baseAddress;
        private readonly Lazy<IHttp> _http;
        private readonly ILoggerAdapter<ISinchConversationMessages>? _logger;
        private readonly string _projectId;

        public Messages(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationMessages>? logger,
            Lazy<IHttp> http)
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
            return _http.Value.Send<SendMessageRequest, SendMessageResponse>(uri, HttpMethod.Post, request,
                cancellationToken: cancellationToken);
        }

        //TODO: add simplified send text to app of recipient (DEVEXP-1243)

        /// <inheritdoc/>  
        public Task<ConversationMessage> Get(string messageId, MessageSource? messagesSource = default,
            CancellationToken cancellationToken = default)
        {
            var param = GetMessageSourceQueryParam(messagesSource);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/messages/{messageId}{param}");

            _logger?.LogDebug("Getting a message with {messageId}...", messageId);
            return _http.Value.Send<ConversationMessage>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public Task<ListMessagesResponse> List(CancellationToken cancellationToken = default)
            => List(new ListMessagesRequest(), cancellationToken);

        /// <inheritdoc/>  
        public Task<ListMessagesResponse> List(ListMessagesRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Fetching list of messages {request}", request);
            var uri = new Uri(_baseAddress,
                $"v1/projects/{_projectId}/messages?{Utils.ToSnakeCaseQueryString(request)}");
            return _http.Value.Send<ListMessagesResponse>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>  
        public Task Delete(string messageId, MessageSource? messagesSource = default,
            CancellationToken cancellationToken = default)
        {
            var param = GetMessageSourceQueryParam(messagesSource);
            _logger?.LogDebug("Deleting a message {messageId}", messageId);
            var uri = new Uri(_baseAddress,
                $"v1/projects/{_projectId}/messages/{messageId}{param}");
            return _http.Value.Send<EmptyResponse>(uri, HttpMethod.Delete, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>  
        public Task<ListMessagesResponse> ListLastMessagesByChannelIdentity(
            ListMessagesByChannelIdentityRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/messages:fetch-last-message");
            _logger?.LogDebug("Fetching messages by channel identity...");
            return _http.Value.Send<ListMessagesByChannelIdentityRequest, ListMessagesResponse>(uri, HttpMethod.Post,
                request, cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>  
        public async IAsyncEnumerable<ConversationMessage> ListLastMessagesByChannelIdentityAuto(
            ListMessagesByChannelIdentityRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto fetching messages by channel identity...");
            do
            {
                var response = await ListLastMessagesByChannelIdentity(request, cancellationToken);
                request.PageToken = response.NextPageToken;
                if (response.Messages == null) continue;
                foreach (var message in response.Messages)
                    yield return message;
            } while (!string.IsNullOrEmpty(request.PageToken));
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<ConversationMessage> ListAuto(CancellationToken cancellationToken = default)
            => ListAuto(new ListMessagesRequest(), cancellationToken);

        /// <inheritdoc/>
        public async IAsyncEnumerable<ConversationMessage> ListAuto(ListMessagesRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto fetching list of messages...");
            do
            {
                var response = await List(request, cancellationToken);
                request.PageToken = response.NextPageToken;
                if (response.Messages == null) continue;
                foreach (var message in response.Messages)
                    yield return message;
            } while (!string.IsNullOrEmpty(request.PageToken));
        }

        /// <inheritdoc/>
        public Task<ConversationMessage> Update(string messageId, string metadata,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Updating message {messageId}...", messageId);
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/messages/{messageId}");
            return _http.Value.Send<UpdateMessageRequest, ConversationMessage>(uri, HttpMethod.Patch,
                new UpdateMessageRequest { Metadata = metadata }, cancellationToken);
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
