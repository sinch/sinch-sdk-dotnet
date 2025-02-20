using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Verification.Common;
using Sinch.Verification.Status;

namespace Sinch.Verification
{
    public interface ISinchVerificationStatus
    {
        /// <summary>
        ///     Queries the verification result by sending the verification ID.
        ///     With this query you can get the result of a verification.
        /// </summary>
        /// <param name="id">The ID of the verification.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SmsVerificationStatusResponse> GetSmsById(string id, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsById" />
        Task<CalloutVerificationStatusResponse> GetCalloutById(string id, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsById" />
        Task<FlashCallVerificationStatusResponse> GetFlashCallById(string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Queries the verification result by sending the verification
        ///     Identity (usually a phone number) and its method.
        ///     With this query you can get the result of a verification.
        /// </summary>
        /// <param name="endpoint">For type number use a E.164-compatible phone number.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SmsVerificationStatusResponse> GetSmsByIdentity(string endpoint,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsByIdentity" />
        Task<CalloutVerificationStatusResponse> GetCalloutByIdentity(string endpoint,
            CancellationToken cancellationToken = default);

        Task<FlashCallVerificationStatusResponse> GetFlashcallByIdentity(string endpoint,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Queries the verification result by sending the verification Reference.
        ///     With this query you can get the result of a verification.
        /// </summary>
        /// <param name="reference">The custom reference of the verification.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SmsVerificationStatusResponse> GetSmsByReference(string reference,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsByReference" />
        Task<FlashCallVerificationStatusResponse> GetFlashcallByReference(string reference,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetSmsByReference" />
        Task<CalloutVerificationStatusResponse> GetCalloutByReference(string reference,
            CancellationToken cancellationToken = default);
    }

    internal sealed class SinchVerificationStatus : ISinchVerificationStatus
    {
        private readonly ILoggerAdapter<SinchVerificationStatus>? _logger;
        private readonly Uri _baseAddress;
        private readonly IHttp _http;

        public SinchVerificationStatus(ILoggerAdapter<SinchVerificationStatus>? logger, Uri baseAddress, IHttp http)
        {
            _http = http;
            _baseAddress = baseAddress;
            _logger = logger;
        }

        private Task<IVerificationStatusResponse> GetById(string id, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/id/{id}");
            _logger?.LogDebug("Getting status of the verification by {id}", id);
            return _http.Send<IVerificationStatusResponse>(uri, HttpMethod.Get,
                cancellationToken: cancellationToken);
        }

        private Task<IVerificationStatusResponse> GetByIdentity(string endpoint, VerificationMethod method,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/{method.Value}/number/{endpoint}");
            _logger?.LogDebug("Getting status of the verification by identity {endpoint} and {method}", endpoint,
                method);
            return _http.Send<IVerificationStatusResponse>(uri, HttpMethod.Get,
                cancellationToken: cancellationToken);
        }

        private Task<IVerificationStatusResponse> GetByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/reference/{reference}");
            _logger?.LogDebug("Getting status of the verification by {reference}", reference);
            return _http.Send<IVerificationStatusResponse>(uri, HttpMethod.Get,
                cancellationToken: cancellationToken);
        }

        public async Task<SmsVerificationStatusResponse> GetSmsById(string id,
            CancellationToken cancellationToken = default)
        {
            var result = await GetById(id, cancellationToken);
            return (result as SmsVerificationStatusResponse)!;
        }

        public async Task<CalloutVerificationStatusResponse> GetCalloutById(string id,
            CancellationToken cancellationToken = default)
        {
            var result = await GetById(id, cancellationToken);
            return (result as CalloutVerificationStatusResponse)!;
        }

        public async Task<FlashCallVerificationStatusResponse> GetFlashCallById(string id,
            CancellationToken cancellationToken = default)
        {
            var result = await GetById(id, cancellationToken);
            return (result as FlashCallVerificationStatusResponse)!;
        }

        public async Task<SmsVerificationStatusResponse> GetSmsByIdentity(string endpoint,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByIdentity(endpoint, VerificationMethod.Sms, cancellationToken);
            return (result as SmsVerificationStatusResponse)!;
        }

        public async Task<CalloutVerificationStatusResponse> GetCalloutByIdentity(string endpoint,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByIdentity(endpoint, VerificationMethod.Callout, cancellationToken);
            return (result as CalloutVerificationStatusResponse)!;
        }

        public async Task<FlashCallVerificationStatusResponse> GetFlashcallByIdentity(string endpoint,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByIdentity(endpoint, VerificationMethod.FlashCall, cancellationToken);
            return (result as FlashCallVerificationStatusResponse)!;
        }

        public async Task<SmsVerificationStatusResponse> GetSmsByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByReference(reference, cancellationToken);
            return (result as SmsVerificationStatusResponse)!;
        }

        public async Task<FlashCallVerificationStatusResponse> GetFlashcallByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByReference(reference, cancellationToken);
            return (result as FlashCallVerificationStatusResponse)!;
        }

        public async Task<CalloutVerificationStatusResponse> GetCalloutByReference(string reference,
            CancellationToken cancellationToken = default)
        {
            var result = await GetByReference(reference, cancellationToken);
            return (result as CalloutVerificationStatusResponse)!;
        }
    }
}
