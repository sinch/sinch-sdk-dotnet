﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Verification.Report;
using Sinch.Verification.Report.Request;
using Sinch.Verification.Report.Response;
using Sinch.Verification.Start.Request;
using Sinch.Verification.Start.Response;

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
        Task<IVerificationStartResponse> Start(VerificationStartRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Report the received verification code to verify it,
        ///     using the identity of the user (in most cases, the phone number).
        ///     For an SMS PIN verification or Phone Call verification, this is the OTP code.
        ///     For flashcalls, this is the CLI.
        /// </summary>
        /// <param name="endpoint">For type number use a E.164-compatible phone number.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IVerificationReportResponse> ReportIdentity(string endpoint, VerifyReportRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Report the received verification code to verify it, using the Verification ID of the Verification request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IVerificationReportResponse> ReportId(string id, VerifyReportRequest request,
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
        public Task<IVerificationStartResponse> Start(VerificationStartRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications");
            _logger?.LogDebug("Starting verification...");
            return _http.Send<VerificationStartRequest, IVerificationStartResponse>(uri, HttpMethod.Post, request,
                cancellationToken: cancellationToken);
        }

        public Task<IVerificationReportResponse> ReportIdentity(string endpoint, VerifyReportRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/number/{endpoint}");
            _logger?.LogDebug("Reporting the the code with identity...");

            return Report(request, uri, cancellationToken);
        }

        public Task<IVerificationReportResponse> ReportId(string id, VerifyReportRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Reporting the the code with id...");
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/id/{id}");

            return Report(request, uri, cancellationToken);
        }

        private Task<IVerificationReportResponse> Report(VerifyReportRequest request,
            Uri uri, CancellationToken cancellationToken)
        {
            return request switch
            {
                FlashCallVerificationReportRequest flashCallVerificationReportRequest =>
                    _http.Send<FlashCallVerificationReportRequest, IVerificationReportResponse>(uri, HttpMethod.Put,
                        flashCallVerificationReportRequest,
                        cancellationToken: cancellationToken),
                SmsVerificationReportRequest smsVerificationRequest => _http
                    .Send<SmsVerificationReportRequest, IVerificationReportResponse>(
                        uri, HttpMethod.Put,
                        smsVerificationRequest,
                        cancellationToken: cancellationToken),
                PhoneCallVerificationReportRequest phoneRequest => _http
                    .Send<PhoneCallVerificationReportRequest, IVerificationReportResponse>(uri, HttpMethod.Put,
                        phoneRequest,
                        cancellationToken: cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(request))
            };
        }
    }
}
