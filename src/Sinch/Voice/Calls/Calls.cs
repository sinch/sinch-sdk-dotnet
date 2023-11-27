﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sinch.Voice.Calls
{
    /// <summary>
    ///     Using the Calls endpoint, you can manage on-going calls or retrieve information about a call.
    /// </summary>
    public interface ISinchVoiceCalls
    {
        /// <summary>
        ///     This method is used to manage ongoing, connected calls. This method uses SVAML in the request body to perform
        ///     various tasks related to the call. For more information about SVAML, see the
        ///     <see href="https://developers.sinch.com/docs/voice/api-reference/svaml/">Callback API</see> documentation.
        ///     <br /><br />
        ///     This method can only be used for calls that originate from or terminate to PSTN or SIP networks.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Successful task if response code is 204</returns>
        Task Update(UpdateCallRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     ou can retrieve information about an ongoing or completed call using a call ID. You can find the call ID of an
        ///     ongoing call by viewing the response object from a callout request. You can find the call ID of a completed call by
        ///     looking at your call logs in your Sinch <see href="https://dashboard.sinch.com/voice/logs">Dashboard</see>.
        ///     <br /><br />
        ///     Note: You can only use this method for calls that terminate to PSTN or SIP networks from an In-app call.
        /// </summary>
        /// <param name="callId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Call> Get(string callId, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc />
    internal class Calls : ISinchVoiceCalls
    {
        /// <inheritdoc />
        public Task Update(UpdateCallRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Call> Get(string callId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
