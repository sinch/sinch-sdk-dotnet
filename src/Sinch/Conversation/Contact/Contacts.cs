using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Contact
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
                cancellationToken: cancellationToken);
        }
    }
}
