using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Voice.Conferences.Get;
using Sinch.Voice.Conferences.ManageParticipants;

namespace Sinch.Voice.Conferences
{
    /// <summary>
    ///     Using the Conferences endpoint, you can perform tasks like retrieving information about an on-going conference,
    ///     muting or unmuting participants, or removing participants from a conference.
    /// </summary>
    public interface ISinchVoiceConferences
    {
        /// <summary>
        ///     Returns information about a conference that matches the provided conference ID.
        /// </summary>
        /// <param name="conferenceId">The unique identifier of the conference. The user sets this value.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetConferenceResponse> Get(string conferenceId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes all participants from a conference.
        /// </summary>
        /// <param name="conferenceId">The unique identifier of the conference. The user sets this value.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Successful task if response code is 204</returns>
        Task KickAll(string conferenceId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes all participants from a conference.
        /// </summary>
        /// <param name="callId">The unique identifier of the call. This value is generated by the system.</param>
        /// <param name="conferenceId">The unique identifier of the conference. The user sets this value.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Successful task if response code is 200</returns>
        Task KickParticipant(string callId, string conferenceId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes all participants from a conference.
        /// </summary>
        /// <param name="callId">The unique identifier of the call. This value is generated by the system.</param>
        /// <param name="conferenceId">The unique identifier of the conference. The user sets this value.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Successful task if response code is 200</returns>
        Task ManageParticipant(string callId, string conferenceId, ManageParticipantRequest request,
            CancellationToken cancellationToken = default);
    }


    /// <inheritdoc />
    internal class SinchConferences : ISinchVoiceConferences
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<ISinchVoiceConferences>? _logger;

        public SinchConferences(ILoggerAdapter<ISinchVoiceConferences>? logger, Uri baseAddress, IHttp http)
        {
            _logger = logger;
            _baseAddress = baseAddress;
            _http = http;
        }

        /// <inheritdoc />
        public Task<GetConferenceResponse> Get(string conferenceId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"calling/v1/conferences/id/{conferenceId}");
            _logger?.LogDebug("Getting info about a conference with {conferenceId}", conferenceId);
            return _http.Send<GetConferenceResponse>(uri, HttpMethod.Get,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task KickAll(string conferenceId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"calling/v1/conferences/id/{conferenceId}");
            _logger?.LogDebug("Kicking all from a conference with {conferenceId}", conferenceId);
            return _http.Send<EmptyResponse>(uri, HttpMethod.Delete,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task ManageParticipant(string callId, string conferenceId, ManageParticipantRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"calling/v1/conferences/id/{conferenceId}/{callId}");
            _logger?.LogDebug("Managing {callId} of {conferenceId}", callId, conferenceId);
            return _http.Send<ManageParticipantRequest, EmptyResponse>(uri, HttpMethod.Patch, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public Task KickParticipant(string callId, string conferenceId, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"calling/v1/conferences/id/{conferenceId}/{callId}");
            _logger?.LogDebug("Kicking {callId} from {conferenceId}", callId, conferenceId);
            return _http.Send<EmptyResponse>(uri, HttpMethod.Delete,
                cancellationToken);
        }
    }
}
