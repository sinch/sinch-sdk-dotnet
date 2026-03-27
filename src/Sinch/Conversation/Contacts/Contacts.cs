using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Common;
using Sinch.Conversation.Contacts.Create;
using Sinch.Conversation.Contacts.GetChannelProfile;
using Sinch.Conversation.Contacts.List;
using Sinch.Conversation.Contacts.Merge;
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
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The contact matching the given ID.</returns>
        Task<Contact> Get(string contactId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Most Conversation API contacts are <a href="https://developers.sinch.com/docs/conversation/contact-management/">created
        ///     automatically</a> when a message is sent to a new
        ///     recipient. You can also create a new contact manually using this API call.
        /// </summary>
        /// <param name="request">Contact details including at least one channel identity and the preferred language.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The newly created contact.</returns>
        Task<Contact> Create(CreateContactRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Lists contacts in the project using server-default pagination (page size 10). Note that, if a WhatsApp contact is
        ///     returned, the display_name field of that contact may be populated with the WhatsApp display name (if the name is
        ///     already stored on the server and the display_name field has not been overwritten by the user).
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>
        ///     A page of contacts using server-default pagination. Use <see cref="ListContactsResponse.NextPageToken" /> to retrieve
        ///     subsequent pages, or use <see cref="ListAuto(ListContactsRequest, CancellationToken)" /> to iterate all pages automatically.
        /// </returns>
        Task<ListContactsResponse> List(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Lists contacts in the project with optional filters and pagination. Note that, if a WhatsApp contact is returned,
        ///     the display_name field of that contact may be populated with the WhatsApp display name (if the name is already
        ///     stored on the server and the display_name field has not been overwritten by the user).
        /// </summary>
        /// <param name="request">Filters and pagination options, including channel, identity, and external ID.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A page of contacts matching the given filters.</returns>
        Task<ListContactsResponse> List(ListContactsRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     See <see cref="List(ListContactsRequest, CancellationToken)" />, but lists all contacts automatically.
        /// </summary>
        /// <param name="request">Filters and pagination options used as the initial request; page tokens are managed automatically.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>An async stream of all <see cref="Contact" /> items across all pages.</returns>
        IAsyncEnumerable<Contact> ListAuto(ListContactsRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     See <see cref="List(CancellationToken)" />, but lists all contacts automatically.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>An async stream of all <see cref="Contact" /> items across all pages.</returns>
        IAsyncEnumerable<Contact> ListAuto(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete a contact as specified by the contact ID.
        /// </summary>
        /// <param name="contactId">The unique ID of the contact to delete.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        Task Delete(string contactId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get user profile from a specific channel. Only supported on MESSENGER, INSTAGRAM, VIBER and LINE channels. Note
        ///     that, in order to retrieve a WhatsApp display name, you can use the Get a Contact or List Contacts operations,
        ///     which will populate the display_name field of each returned contact with the WhatsApp display name (if the name is
        ///     already stored on the server and the display_name field has not been overwritten by the user).
        /// </summary>
        /// <param name="request">The app ID, recipient, and target channel for the profile lookup.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The channel profile, including the user's display name on that channel.</returns>
        Task<ChannelProfile> GetChannelProfile(GetChannelProfileRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get user profile from a specific channel by contact ID.
        ///     Convenience helper for <see cref="GetChannelProfile(GetChannelProfileRequest, CancellationToken)" />.
        /// </summary>
        /// <param name="appId">The ID of the app.</param>
        /// <param name="channel">The channel to get the profile from.</param>
        /// <param name="contactId">The ID of the contact.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ChannelProfile> GetChannelProfileByContactId(string appId, ChannelProfileConversationChannel channel,
            string contactId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get user profile from a specific channel by channel identities.
        ///     Convenience helper for <see cref="GetChannelProfile(GetChannelProfileRequest, CancellationToken)" />.
        /// </summary>
        /// <param name="appId">The ID of the app.</param>
        /// <param name="channel">The channel to get the profile from.</param>
        /// <param name="channelIdentities">One or more channel identities to look up.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns></returns>
        Task<ChannelProfile> GetChannelProfileByChannelIdentity(string appId, ChannelProfileConversationChannel channel,
            IEnumerable<ChannelIdentity> channelIdentities, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get user profile from a specific channel by a single channel identity.
        ///     Convenience helper for <see cref="GetChannelProfileByChannelIdentity(string, ChannelProfileConversationChannel, IEnumerable{ChannelIdentity}, CancellationToken)" />.
        /// </summary>
        /// <param name="appId">The ID of the app.</param>
        /// <param name="channel">The channel to get the profile from.</param>
        /// <param name="channelIdentity">The channel identity to look up.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ChannelProfile> GetChannelProfileByChannelIdentity(string appId, ChannelProfileConversationChannel channel,
            ChannelIdentity channelIdentity, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates a contact as specified by the contact ID.
        /// </summary>
        /// <param name="contact">
        ///     The contact to update. Only fields that are set (non-null) are sent to the server;
        ///     unset fields are left unchanged.
        /// </param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The updated contact.</returns>
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
        /// <param name="request">The merge request containing the source contact ID and optional merge strategy.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The merged destination contact with all combined identities and conversations.</returns>
        Task<Contact> MergeContact(string destinationId, MergeContactRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Lists identity conflicts for the project using server-default pagination. An identity conflict occurs when the same
        ///     channel identity is linked to multiple contacts, which may lead to ambiguous message delivery.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>
        ///     A page of identity conflicts using server-default pagination. Use
        ///     <see cref="ListIdentityConflictsResponse.NextPageToken" /> to retrieve subsequent pages, or use
        ///     <see cref="ListIdentityConflictsAuto(ListIdentityConflictsRequest, CancellationToken)" /> to iterate all pages automatically.
        /// </returns>
        Task<ListIdentityConflictsResponse> ListIdentityConflicts(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Lists identity conflicts for the project with optional pagination. An identity conflict occurs when the same
        ///     channel identity is linked to multiple contacts, which may lead to ambiguous message delivery.
        /// </summary>
        /// <param name="request">Pagination options: page size (max 20) and optional page token for subsequent pages.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A page of identity conflicts, plus a token for the next page when more results are available.</returns>
        Task<ListIdentityConflictsResponse> ListIdentityConflicts(ListIdentityConflictsRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     See <see cref="ListIdentityConflicts(ListIdentityConflictsRequest, CancellationToken)" />, but lists all identity conflicts automatically.
        /// </summary>
        /// <param name="request">Pagination options used as the initial request; page tokens are managed automatically.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>An async stream of all <see cref="IdentityConflict" /> items across all pages.</returns>
        IAsyncEnumerable<IdentityConflict> ListIdentityConflictsAuto(ListIdentityConflictsRequest request,
            CancellationToken cancellationToken = default);
    }

    internal sealed class Contacts : ISinchConversationContacts
    {
        private readonly Uri _baseAddress;
        private readonly Lazy<IHttp> _http;
        private readonly ILoggerAdapter<ISinchConversationContacts>? _logger;
        private readonly string _projectId;

        public Contacts(string projectId, Uri baseAddress, ILoggerAdapter<ISinchConversationContacts>? logger,
            Lazy<IHttp> http)
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
            return _http.Value.Send<Contact>(uri, HttpMethod.Get,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<Contact> Create(CreateContactRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts");
            _logger?.LogDebug("Creating a contact for a {projectId}", _projectId);
            return _http.Value.Send<CreateContactRequest, Contact>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListContactsResponse> List(CancellationToken cancellationToken = default)
            => List(new ListContactsRequest(), cancellationToken);

        /// <inheritdoc />
        public Task<ListContactsResponse> List(ListContactsRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = Utils.ToSnakeCaseQueryString(request);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts?{query}");
            _logger?.LogDebug("Listing contacts for {projectId}", _projectId);
            return _http.Value.Send<ListContactsResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<Contact> ListAuto(CancellationToken cancellationToken = default)
            => ListAuto(new ListContactsRequest(), cancellationToken);

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
                    await _http.Value.Send<ListContactsResponse>(uri, HttpMethod.Get, cancellationToken);
                request.PageToken = response.NextPageToken;
                if (response.Contacts == null) continue;
                foreach (var contact in response.Contacts)
                    yield return contact;
            } while (!string.IsNullOrEmpty(request.PageToken));
        }

        /// <inheritdoc />
        public Task Delete(string contactId, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Deleting a {contactId} from {projectId}", contactId, _projectId);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts/{contactId}");
            return _http.Value.Send<EmptyResponse>(uri, HttpMethod.Delete, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ChannelProfile> GetChannelProfile(GetChannelProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Getting a profile for {projectId} of {channel}", _projectId, request.Channel);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts:getChannelProfile");
            return _http.Value.Send<GetChannelProfileRequest, ChannelProfile>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<ChannelProfile> GetChannelProfileByContactId(string appId,
            ChannelProfileConversationChannel channel, string contactId,
            CancellationToken cancellationToken = default)
            => GetChannelProfile(new GetChannelProfileRequest
            {
                AppId = appId,
                Channel = channel,
                Recipient = new ContactRecipient { ContactId = contactId }
            }, cancellationToken);

        /// <inheritdoc />
        /// <inheritdoc />
        public Task<ChannelProfile> GetChannelProfileByChannelIdentity(string appId,
            ChannelProfileConversationChannel channel, IEnumerable<ChannelIdentity> channelIdentities,
            CancellationToken cancellationToken = default)
            => GetChannelProfile(new GetChannelProfileRequest
            {
                AppId = appId,
                Channel = channel,
                Recipient = new Identified
                {
                    IdentifiedBy = new IdentifiedBy { ChannelIdentities = channelIdentities.ToList() }
                }
            }, cancellationToken);

        /// <inheritdoc />
        public Task<ChannelProfile> GetChannelProfileByChannelIdentity(string appId,
            ChannelProfileConversationChannel channel, ChannelIdentity channelIdentity,
            CancellationToken cancellationToken = default)
            => GetChannelProfileByChannelIdentity(appId, channel, [channelIdentity], cancellationToken);

        /// <inheritdoc />
        public Task<Contact> Update(Contact contact, CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Updating a {contactId} of {projectId}", contact.Id, _projectId);
            // the update_mask param will regulate which properties to set.
            // Keep in mind that no depth is supported: for example, you cannot mask channel_identities.identity 
            var uri = new Uri(_baseAddress,
                $"/v1/projects/{_projectId}/contacts/{contact.Id}?update_mask={contact.GetPropertiesMask()}");
            return _http.Value.Send<Contact, Contact>(uri, HttpMethod.Patch, contact,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task<Contact> MergeContact(string destinationId, MergeContactRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Merging contacts from {sourceId} to {destinationId} for {projectId}", request.SourceId,
                destinationId, _projectId);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts/{destinationId}:merge");
            return _http.Value.Send<MergeContactRequest, Contact>(uri, HttpMethod.Post, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListIdentityConflictsResponse> ListIdentityConflicts(CancellationToken cancellationToken = default)
            => ListIdentityConflicts(new ListIdentityConflictsRequest(), cancellationToken);

        /// <inheritdoc />
        public Task<ListIdentityConflictsResponse> ListIdentityConflicts(ListIdentityConflictsRequest request,
            CancellationToken cancellationToken = default)
        {
            var query = Utils.ToSnakeCaseQueryString(request);
            var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts:identityConflicts?{query}");

            _logger?.LogDebug("Listing identity conflicts for {projectId}", _projectId);

            return _http.Value.Send<ListIdentityConflictsResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<IdentityConflict> ListIdentityConflictsAuto(ListIdentityConflictsRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto listing identity conflicts for {projectId}", _projectId);

            do
            {
                var query = Utils.ToSnakeCaseQueryString(request);
                var uri = new Uri(_baseAddress, $"/v1/projects/{_projectId}/contacts:identityConflicts?{query}");
                var response = await _http.Value.Send<ListIdentityConflictsResponse>(uri, HttpMethod.Get, cancellationToken);

                request.PageToken = response.NextPageToken;

                if (response.Conflicts == null)
                    continue;

                foreach (var conflict in response.Conflicts)
                    yield return conflict;

            } while (!string.IsNullOrEmpty(request.PageToken));
        }
    }
}
