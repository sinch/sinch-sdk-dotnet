using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Contacts.Create;
using Sinch.Conversation.Contacts.GetChannelProfile;
using Sinch.Conversation.Contacts.List;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Contacts
{
    /// <summary>
    ///     A contact is a collection that groups together underlying connected channel recipient identities. It's tied to a
    ///     specific project and is therefore considered public to all apps sharing the same project. Most contact creation and
    ///     maintenance is handled by the Conversation API's automatic [contact
    ///     management](https://developers.sinch.com/docs/conversation/contact-management/ processes. However, you can also use
    ///     API calls to manually manage your contacts.<br /><br />
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Field</term>
    ///             <description>Description</description>
    ///         </listheader>
    ///         <item>
    ///             <term>Channel identities</term>
    ///             <description>List of channel identities specifying how the contact is identified on underlying channels</description>
    ///         </item>
    ///         <item>
    ///             <term>Channel priority</term>
    ///             <description>
    ///                 Specifies the channel priority order used when sending messages to this contact. This can be
    ///                 overridden by message specific channel priority order.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Display name</term>
    ///             <description>Optional display name used in chat windows and other UIs</description>
    ///         </item>
    ///         <item>
    ///             <term>Email</term>
    ///             <description>Optional Email of the contact</description>
    ///         </item>
    ///         <item>
    ///             <term>External id</term>
    ///             <description>Optional identifier of the contact in external systems</description>
    ///         </item>
    ///         <item>
    ///             <term>Metadata</term>
    ///             <description>Optional metadata associated with the contact.</description>
    ///         </item>
    ///     </list>
    /// </summary>
    public interface ISinchConversationContacts
    {
        /// <summary>
        ///     Returns a specific contact as specified by the contact ID. Note that, if a WhatsApp contact is returned, the
        ///     display_name field of that contact may be populated with the WhatsApp display name (if the name is already stored
        ///     on the server and the display_name field has not been overwritten by the user).
        /// </summary>
        /// <param name="contactId">The unique ID of the contact.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Contact> Get(string contactId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Most Conversation API contacts are [created
        ///     automatically](https://developers.sinch.com/docs/conversation/contact-management/) when a message is sent to a new
        ///     recipient. You can also create a new contact manually using this API call.
        /// </summary>
        /// <returns></returns>
        Task<Contact> Create(CreateContactRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     List all contacts in the project. Note that, if a WhatsApp contact is returned, the display_name field of that
        ///     contact may be populated with the WhatsApp display name (if the name is already stored on the server and the
        ///     display_name field has not been overwritten by the user).
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListContactsResponse> List(ListContactsRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     See <see cref="List" />, but lists all contacts automatically.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<Contact> ListAuto(ListContactsRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete a contact as specified by the contact ID.
        /// </summary>
        /// <param name="contactId">The unique ID of the contact.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string contactId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get user profile from a specific channel. Only supported on MESSENGER, INSTAGRAM, VIBER and LINE channels. Note
        ///     that, in order to retrieve a WhatsApp display name, you can use the Get a Contact or List Contacts operations,
        ///     which will populate the display_name field of each returned contact with the WhatsApp display name (if the name is
        ///     already stored on the server and the display_name field has not been overwritten by the user).
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ChannelProfile> GetChannelProfile(GetChannelProfileRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates a contact as specified by the contact ID.
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Contact> Update(Contact contact, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Merge two contacts. The remaining contact will contain all conversations that the removed contact did. If both
        ///     contacts had conversations within the same App, messages from the removed contact will be merged into corresponding
        ///     active conversations in the destination contact. Channel identities will be moved from the source contact to the
        ///     destination contact only for channels that weren't present there before. Moved channel identities will be placed at
        ///     the bottom of the channel priority list. Optional fields from the source contact will be copied only if
        ///     corresponding fields in the destination contact are empty The contact being removed cannot be referenced after this
        ///     call.
        /// </summary>
        /// <param name="destinationId">The unique ID of the contact that should be kept when merging two contacts.</param>
        /// <param name="sourceId">The ID of the contact that should be removed.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Contact> Merge(string destinationId, string sourceId, CancellationToken cancellationToken = default);
    }

    internal class Contacts : ISinchConversationContacts
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchConversationContacts> _logger;
        private readonly string _projectId;

        public Contacts(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationContacts> logger,
            IHttp http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<Contact> Get(string contactId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts/{contactId}");
            _logger?.LogDebug("Getting a {contactId} for a {projectId}", contactId, _projectId);
            return _http.Send<Contact>(uri, HttpMethod.Get,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<Contact> Create(CreateContactRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts");
            _logger?.LogDebug("Creating a contact for a {projectId}", _projectId);
            return _http.Send<CreateContactRequest, Contact>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListContactsResponse> List(ListContactsRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = Utils.ToSnakeCaseQueryString(request);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts?{query}");
            _logger?.LogDebug("Listing contacts for {projectId}", _projectId);
            return _http.Send<ListContactsResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<Contact> ListAuto(ListContactsRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto Listing contacts for {projectId}", _projectId);
            do
            {
                var query = Utils.ToSnakeCaseQueryString(request);
                var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts?{query}");
                var response =
                    await _http.Send<ListContactsResponse>(uri, HttpMethod.Get, cancellationToken);
                request.PageToken = response.NextPageToken;
                foreach (var contact in response.Contacts) yield return contact;
            } while (request.PageToken is not null);
        }

        /// <inheritdoc />
        public Task Delete(string contactId, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Deleting a {contactId} from {projectId}", contactId, _projectId);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts/{contactId}");
            return _http.Send<object>(uri, HttpMethod.Delete, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ChannelProfile> GetChannelProfile(GetChannelProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting a profile for {projectId} of {channel}", _projectId, request.Channel);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts:getChannelProfile");
            return _http.Send<GetChannelProfileRequest, ChannelProfile>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<Contact> Update(Contact contact, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Updating a {contactId} of {projectId}", contact.Id, _projectId);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts/{contact.Id}");
            return _http.Send<Contact, Contact>(uri, HttpMethod.Patch, contact,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<Contact> Merge(string destinationId, string sourceId, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Merging contacts from {sourceId} to {destinationId} for {projectId}", sourceId,
                destinationId, _projectId);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts/{destinationId}:merge");
            return _http.Send<object, Contact>(uri, HttpMethod.Post, new
                {
                    source_id = sourceId,
                    // NOTE: keep in mind while this enum has only one value, it can change in the future.
                    strategy = "MERGE"
                },
                cancellationToken);
        }
    }
}
