using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.SMS.Batches.DryRun;
using Sinch.SMS.Batches.List;
using Sinch.SMS.Batches.Send;
using Sinch.SMS.Batches.Update;

namespace Sinch.SMS.Batches
{
    public interface ISinchSmsBatches
    {
        /// <summary>
        ///     Send a message or a batch of messages. <br /><br />
        ///     Depending on the length of the body, one message might  be split into multiple parts and charged accordingly.
        ///     <br /><br />
        ///     Any groups targeted in a scheduled batch will be evaluated at the time of sending. If a group is deleted between
        ///     batch creation and scheduled date, it will be considered empty.<br /><br />
        ///     Be sure to use the correct region in the SmsOptions
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IBatch> Send(ISendBatchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     With the list operation you can list batch messages created in the last 14 days that you have created.
        ///     This operation supports pagination.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ListBatchesResponse> List(ListBatchesRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     With the list operation you can list batch messages created in the last 14 days that you have created.
        ///     This operation supports pagination.
        ///
        ///     This method will yield all the batches.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<IBatch> ListAuto(ListBatchesRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation will perform a dry run of a batch which calculates
        ///     the bodies and number of parts for all messages in the batch without actually sending any messages.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DryRunResponse> DryRun(DryRunRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation returns a specific batch that matches the provided batch ID.
        /// </summary>
        /// <param name="batchId">The batch ID you received from sending a message.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Batch</returns>
        Task<IBatch> Get(string batchId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation updates all specified parameters of a batch that matches the provided batch ID.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="batchId">The batch ID you received from sending a message.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IBatch> Update(string batchId, IUpdateBatchRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     This operation will replace all the parameters of a batch with the provided values.
        ///     It is the same as cancelling a batch and sending a new one instead.
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="batch"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IBatch> Replace(string batchId, ISendBatchRequest batch, CancellationToken cancellationToken = default);

        /// <summary>
        ///     A batch can be canceled at any point.
        ///     If a batch is canceled while it's currently being delivered
        ///     some messages currently being processed might still be delivered.
        ///     The delivery report will indicate which messages were canceled and which weren't.<br /><br />
        ///     Canceling a batch scheduled in the future will result in an empty delivery report while canceling an already sent
        ///     batch would result in no change to the completed delivery report.
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IBatch> Cancel(string batchId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Send feedback if your system can confirm successful message delivery. <br /><br />
        ///     Feedback can only be provided if feedback_enabled was set when batch was submitted. <br /><br />
        ///     Batches: It is possible to submit feedback multiple times for the same batch for different recipients.
        ///     Feedback without specified recipients is treated as successful message delivery
        ///     to all recipients referenced in the batch.
        ///     Note that the recipients key is still required even if the value is empty.<br /><br />
        ///     Groups: If the batch message was creating using a group ID, at least one recipient is required.
        ///     Excluding recipients (an empty recipient list) does not work and will result in a failed request.
        /// </summary>
        /// <param name="batchId">The batch ID you received from sending a message.</param>
        /// <param name="recipients">
        ///     A list of phone numbers (MSISDNs) that have successfully received the message.
        ///     Can be empty.
        ///     If the feedback was enabled for a group, at least one phone number is required.
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns>Successful task if the request has been received and is processing</returns>
        Task SendDeliveryFeedback(string batchId, IEnumerable<string> recipients,
            CancellationToken cancellationToken = default);
    }

    internal sealed class Batches : ISinchSmsBatches
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchSmsBatches>? _logger;
        private readonly string _projectOrServicePlanId;

        internal Batches(string projectOrServicePlanId, Uri baseAddress, ILoggerAdapter<ISinchSmsBatches>? logger,
            IHttp http)
        {
            _projectOrServicePlanId = projectOrServicePlanId;
            _http = http;
            _baseAddress = baseAddress;
            _logger = logger;
        }

        public Task<IBatch> Send(ISendBatchRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/batches");
            _logger?.LogDebug("Making a request to {uri}", uri);
            return _http.Send<ISendBatchRequest, IBatch>(uri, HttpMethod.Post, request, cancellationToken);
        }

        public Task<ListBatchesResponse> List(ListBatchesRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/batches?{request.GetQueryString()}");
            _logger?.LogDebug("Listing batches...");
            return _http.Send<ListBatchesResponse>(uri, HttpMethod.Get, cancellationToken);
        }

        public async IAsyncEnumerable<IBatch> ListAuto(ListBatchesRequest request,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Auto Listing batches...");
            bool lastPage;
            do
            {
                var query = request.GetQueryString();
                var relative = $"xms/v1/{_projectOrServicePlanId}/batches";
                if (!string.IsNullOrEmpty(query))
                {
                    relative += "?" + query;
                }

                var uri = new Uri(_baseAddress, relative);
                var response = await _http.Send<ListBatchesResponse>(uri, HttpMethod.Get, cancellationToken);

                if (response.Batches != null)
                    foreach (var batch in response.Batches)
                    {
                        yield return batch;
                    }

                lastPage = Utils.IsLastPage(response.Page, response.PageSize, response.Count);
                request.Page ??= response.Page;
                request.Page++;
            } while (!lastPage);
        }

        public Task<DryRunResponse> DryRun(DryRunRequest request, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress,
                $"xms/v1/{_projectOrServicePlanId}/batches/dry_run?{request.GetQueryString()}");
            _logger?.LogDebug("Performing dry run...");
            return _http.Send<ISendBatchRequest, DryRunResponse>(uri, HttpMethod.Post, request.BatchRequest,
                cancellationToken);
        }

        public Task<IBatch> Get(string batchId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(batchId))
                throw new ArgumentNullException(nameof(batchId), "BatchId could not be empty");

            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/batches/{batchId}");
            _logger?.LogDebug("Getting batch...");
            return _http.Send<IBatch>(uri, HttpMethod.Get, cancellationToken);
        }

        public Task<IBatch> Update(string batchId, IUpdateBatchRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/batches/{batchId}");
            _logger?.LogDebug("Updating a batch with {id}...", batchId);
            return _http.Send<IUpdateBatchRequest, IBatch>(uri, HttpMethod.Post, request, cancellationToken);
        }

        public Task<IBatch> Replace(string batchId, ISendBatchRequest batch,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/batches/{batchId}");
            _logger?.LogDebug("Replacing a batch with {id}...", batchId);
            return _http.Send<ISendBatchRequest, IBatch>(uri, HttpMethod.Put, batch, cancellationToken);
        }

        public Task<IBatch> Cancel(string batchId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/batches/{batchId}");
            _logger?.LogDebug("Cancelling batch with {id}...", batchId);
            return _http.Send<IBatch>(uri, HttpMethod.Delete, cancellationToken);
        }

        public Task SendDeliveryFeedback(string batchId, IEnumerable<string> recipients,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"xms/v1/{_projectOrServicePlanId}/batches/{batchId}/delivery_feedback");
            _logger?.LogDebug("Sending delivery feedback for batch {id}...", batchId);
            return _http.Send<object, EmptyResponse>(uri, HttpMethod.Post, new
            {
                recipients
            }, cancellationToken);
        }
    }
}
