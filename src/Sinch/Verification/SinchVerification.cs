using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Verification.Start;

namespace Sinch.Verification
{
    public interface ISinchVerification
    {
        /// <summary>
        ///     This method is used by the mobile and web Verification SDKs to start a verification.
        ///     It can also be used to request a verification from your backend, by making an request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IVerificationResponse> Start(VerificationStartRequest request,
            CancellationToken cancellationToken = default);
    }

    internal class SinchVerification : ISinchVerification
    {
        private readonly ILoggerAdapter<SinchVerification> _logger;
        private readonly Uri _baseAddress;
        private readonly IHttp _http;

        public SinchVerification(ILoggerAdapter<SinchVerification> logger, Uri baseAddress, IHttp http)
        {
            _logger = logger;
            _baseAddress = baseAddress;
            _http = http;
        }

        /// <inheritdoc />
        public Task<IVerificationResponse> Start(VerificationStartRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications");
            _logger?.LogDebug("Starting verification...");
            return _http.Send<VerificationStartRequest, IVerificationResponse>(uri, HttpMethod.Post, request,
                cancellationToken);
        }
    }
}
