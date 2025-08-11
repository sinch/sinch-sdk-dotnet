using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Conversation.Apps.Create;
using Sinch.Conversation.Apps.Update;
using Sinch.Core;
using Sinch.Logger;

namespace Sinch.Conversation.Apps
{
    /// <summary>
    ///     Apps are created and configured through
    ///     the <see href="https://dashboard.sinch.com/convapi/getting-started">Sinch Dashboard</see>,
    ///     are tied to the API user and come with a set of channel credentials
    ///     for each underlying connected channel.
    ///     The app has a list of conversations between itself and different contacts which share the same project.
    /// </summary>
    public interface ISinchConversationApps
    {
        /// <summary>
        ///     You can create a new Conversation API app using the API.
        ///     You can create an app for one or more channels at once.
        ///     The ID of the app is generated at creation and will be returned in the response.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<App> Create(CreateAppRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a list of all apps.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<App>> List(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns a particular app as specified by the App ID.
        /// </summary>
        /// <param name="appId">
        ///     The unique ID of the app. You can find this on the
        ///     <see href="https://dashboard.sinch.com/convapi/apps">Sinch Dashboard</see>
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<App> Get(string appId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes the app specified by the App ID. Note that this operation will not delete contacts
        ///     (which are stored at the project level) nor any channel-specific resources
        ///     (for example, WhatsApp Sender Identities will not be deleted).
        /// </summary>
        /// <param name="appId">
        ///     The unique ID of the app. You can find this on the
        ///     <see href="https://dashboard.sinch.com/convapi/apps">Sinch Dashboard</see>
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string appId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates a particular app as specified by the App ID. Note that this is a PATCH operation, so any specified field
        ///     values will replace existing values. Therefore, if you'd like to add additional configurations to an existing
        ///     Conversation API app, ensure that you include existing values AND new values in the call. For example, if you'd
        ///     like to add new channel_credentials, you can get your existing Conversation API app, extract the existing
        ///     channel_credentials list, append your new configuration to that list, and include the updated channel_credentials
        ///     list in this update call.
        /// </summary>
        /// <param name="appId">
        ///     The unique ID of the app. You can find this on the
        ///     <see href="https://dashboard.sinch.com/convapi/apps">Sinch Dashboard</see>
        /// </param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<App> Update(string appId, UpdateAppRequest request, CancellationToken cancellationToken = default);
    }

    internal sealed class Apps : ISinchConversationApps
    {
        private readonly Uri _baseAddress;
        private readonly Lazy<IHttp> _http;
        private readonly ILoggerAdapter<Apps>? _logger;
        private readonly string _projectId;

        public Apps(string projectId, Uri baseAddress, ILoggerAdapter<Apps>? logger, Lazy<IHttp> http)
        {
            _projectId = projectId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<App> Create(CreateAppRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/apps");
            _logger?.LogDebug("Creating an app...");
            return _http.Value.Send<CreateAppRequest, App>(uri, HttpMethod.Post, request,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<App>> List(CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/apps");
            _logger?.LogDebug("Listing apps for a {projectId}", _projectId);
            // flatten the response 
            var response = await _http.Value.Send<ListResponse>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
            return response.Apps ?? new List<App>();
        }

        /// <inheritdoc />
        public Task<App> Get(string appId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(appId))
            {
                _logger?.LogError("Get an app failed. Provided app id was null or empty for a {projectId}",
                    _projectId);
                throw new ArgumentNullException(nameof(appId), "The appId parameter is null or empty");
            }

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/apps/{appId}");
            _logger?.LogDebug("Getting an app for a {projectId} with {appId}", _projectId, appId);
            return _http.Value.Send<App>(uri, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task Delete(string appId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(appId))
            {
                _logger?.LogError("Delete an app failed. Provided app id was null or empty for a {projectId}",
                    _projectId);
                throw new ArgumentNullException(nameof(appId), "The appId parameter is null or empty");
            }

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/apps/{appId}");
            _logger?.LogDebug("Deleting an app for a {projectId} with {appId}", _projectId, appId);
            return _http.Value.Send<EmptyResponse>(uri, HttpMethod.Delete, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public Task<App> Update(string appId, UpdateAppRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(appId))
            {
                _logger?.LogError("Delete an app failed. Provided app id was null or empty for a {projectId}",
                    _projectId);
                throw new ArgumentNullException(nameof(appId), "The appId parameter is null or empty");
            }

            string? query = null;
            if (request.UpdateMaskPaths is not null && request.UpdateMaskPaths.Any())
                query =
                    $"?update_mask.paths={string.Join("&update_mask.paths=", request.UpdateMaskPaths.Select(WebUtility.UrlEncode))}";

            var uri = new Uri(_baseAddress, $"v1/projects/{_projectId}/apps/{appId}{query}");
            _logger?.LogDebug("Updating an app for a {projectId} with {appId}", _projectId, appId);
            return _http.Value.Send<UpdateAppRequest, App>(uri, HttpMethod.Patch, request,
                cancellationToken: cancellationToken);
        }

        private sealed class ListResponse
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            public List<App>? Apps { get; set; }
        }
    }
}
