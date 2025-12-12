using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.SMS.Groups.Create;
using Sinch.SMS.Groups.List;
using Sinch.SMS.Groups.Replace;
using Sinch.SMS.Groups.Update;

namespace Sinch.SMS.Groups
{
    public interface ISinchSmsGroups
    {
        /// <summary>
        ///     With the list operation you can list all groups that you have created. This operation supports pagination.<br />
        ///     <br />
        ///     Groups are returned in reverse chronological order.
        /// </summary>
        /// <param name="request">Request params</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A paging data and a list of groups</returns>
        Task<ListGroupsResponse> List(ListGroupsRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     With the list operation you can list all groups that you have created. This operation supports pagination.<br />
        ///     <br />
        ///     Groups are returned in reverse chronological order.
        /// </summary>
        /// <param name="request">Request params</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An async enumerable of <see cref="Group"/></returns>
        IAsyncEnumerable<Group> ListAuto(ListGroupsRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation retrieves a specific group with the provided group ID.
        /// </summary>
        /// <param name="groupId">ID of a group that you are interested in getting.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Group> Get(string groupId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     A group is a set of phone numbers (MSISDNs) that can be used as a target in the send_batch_msg operation.
        ///     An MSISDN can only occur once in a group and any attempts to add a duplicate would be ignored but not rejected.
        /// </summary>
        /// <param name="request">Params to create a group</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Group> Create(CreateGroupRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     With the update group operation, you can add and remove members in an existing group as well as rename the group.
        ///     <br /><br />
        ///     This method encompasses a few ways to update a group:<br />
        ///     1. By using add and remove arrays containing phone numbers, you control the group movements.
        ///     Any list of valid numbers in E.164 format can be added. <br />
        ///     2. By using the auto_update object, your customer can add or remove themselves from groups. <br />
        ///     3. You can also add or remove other groups into this group with AddFromGroup and RemoveFromGroup. <br />
        ///     Other group update info <br />
        ///     * The request will not be rejected for duplicate adds or unknown removes. <br />
        ///     * The additions will be done before the deletions. If an phone number is on both lists,
        ///     it will not be apart of the resulting group. <br />
        ///     * Updating a group targeted by a batch message scheduled in the future is allowed.
        ///     Changes will be reflected when the batch is sent.
        /// </summary>
        /// <param name="request">Params to create a group</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Group> Update(UpdateGroupRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     The replace operation will replace all parameters, including members,
        ///     of an existing group with new values.<br /><br />
        ///     Replacing a group targeted by a batch message scheduled in the future is
        ///     allowed and changes will be reflected when the batch is sent.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Group> Replace(ReplaceGroupRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation deletes the group with the provided group ID.
        /// </summary>
        /// <param name="groupId">ID of a group to delete.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A successful task</returns>
        Task Delete(string groupId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation retrieves the members of the group with the provided group ID.
        /// </summary>
        /// <param name="groupId">ID of a group to list numbers for</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of phone number in <see href="">E.164</see> format.</returns>
        Task<IEnumerable<string>> ListMembers(string groupId, CancellationToken cancellationToken = default);
    }

    internal sealed class Groups : ISinchSmsGroups
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchSmsGroups>? _logger;
        private readonly string _projectOrServicePlanId;

        internal Groups(string projectOrServicePlanId, Uri baseAddress, ILoggerAdapter<ISinchSmsGroups>? logger,
            IHttp http)
        {
            _projectOrServicePlanId = projectOrServicePlanId;
            _baseAddress = baseAddress;
            _logger = logger;
            _http = http;
        }

        /// <inheritdoc />
        public Task<ListGroupsResponse> List(ListGroupsRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/groups?{request.GetQueryString()}");
            _logger?.LogDebug("Listing groups...");
            return _http.Send<ListGroupsResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<Group> ListAuto(ListGroupsRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto listing groups...");
            bool isLastPage;
            int total = 0;
            do
            {
                var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/groups?{request.GetQueryString()}");
                _logger?.LogDebug("Listing group {page}", request.Page);
                var response = await _http.Send<ListGroupsResponse>(uri, HttpMethod.Get, cancellationToken);
                total += response.PageSize;
                foreach (var group in response.Groups)
                {
                    yield return group;
                }

                isLastPage = total >= response.Count;
                request.Page ??= response.Page;
                request.Page++;
            } while (!isLastPage);
        }

        /// <inheritdoc />
        public Task<Group> Get(string groupId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/groups/{groupId}");
            _logger?.LogDebug("Fetching a group with {groupId}", groupId);
            return _http.Send<Group>(uri, HttpMethod.Get, cancellationToken);
        }

        /// <inheritdoc />
        public Task<Group> Create(CreateGroupRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/groups");
            _logger?.LogDebug("Creating a group...");
            return _http.Send<CreateGroupRequest, Group>(uri, HttpMethod.Post, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<Group> Update(UpdateGroupRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/groups/{request.GroupId}");
            _logger?.LogDebug("Updating a group with {id}...", request.GroupId);
            // No simple way to conditionally remove name property from request object. Easy to go with anonymous objects
            if (request.Name == string.Empty)
                return _http.Send<RequestWithoutName, Group>(uri, HttpMethod.Post, new RequestWithoutName
                {
                    Add = request.Add,
                    Remove = request.Remove,
                    AddFromGroup = request.AddFromGroup,
                    RemoveFromGroup = request.RemoveFromGroup,
                    AutoUpdate = request.AutoUpdate
                }, cancellationToken);

            return _http.Send<IGroupUpdateRequest, Group>(uri, HttpMethod.Post, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<Group> Replace(ReplaceGroupRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/groups/{request.GroupId}");
            _logger?.LogDebug("Replacing a group with {id}...", request.GroupId);
            var requestInner = new RequestInner
            {
                Members = request.Members,
                Name = request.Name
            };
            return _http.Send<RequestInner, Group>(uri, HttpMethod.Put, requestInner, cancellationToken);
        }

        /// <inheritdoc />
        public Task Delete(string groupId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/groups/{groupId}");
            _logger?.LogDebug("Deleting a group with {id}...", groupId);
            return _http.Send<EmptyResponse>(uri, HttpMethod.Delete, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IEnumerable<string>> ListMembers(string groupId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/groups/{groupId}/members");
            _logger?.LogDebug("Listing members of a group with {id}...", groupId);
            return _http.Send<IEnumerable<string>>(uri, HttpMethod.Get, cancellationToken);
        }
    }
}
