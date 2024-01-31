﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Verification.Common;
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
        Task<IStartVerificationResponse> Start(StartVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Starts an SMS Verification. Verification by SMS message with a PIN code.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<StartSmsVerificationResponse> StartSms(StartSmsVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Starts an SMS Verification for the specified E.164-compatible phone number. Verification by SMS message with a PIN
        ///     code.
        /// </summary>
        /// <param name="phoneNumber">A E.164-compatible phone number</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<StartSmsVerificationResponse> StartSms(string phoneNumber,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Starts a Flash Call verification. Verification by placing a flashcall (missed call) and detecting the incoming
        ///     calling number (CLI).
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<StartFlashCallVerificationResponse> StartFlashCall(StartFlashCallVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Starts a Phone Call verification.Verification by placing a PSTN call to the user's phone and playing an
        ///     announcement, asking the user to press a particular digit to verify the phone number
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<StartPhoneCallVerificationResponse> StartPhoneCall(StartPhoneCallVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Starts a Data verification. Verification by accessing internal infrastructure of mobile carriers to verify if given
        ///     verification attempt was originated from device with matching phone number.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<StartDataVerificationResponse> StartSeamless(StartDataVerificationRequest request,
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
        ///     Report the received verification code to verify it, using the Verification ID of the Verification request.
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
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<SinchVerification> _logger;

        public SinchVerification(ILoggerAdapter<SinchVerification> logger, Uri baseAddress, IHttp http)
        {
            _logger = logger;
            _baseAddress = baseAddress;
            _http = http;
        }

        /// <inheritdoc />
        public Task<IStartVerificationResponse> Start(StartVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, "verification/v1/verifications");
            _logger?.LogDebug("Starting verification...");
            return _http.Send<StartVerificationRequest, IStartVerificationResponse>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        /// <inheritdoc />
        public async Task<StartSmsVerificationResponse> StartSms(StartSmsVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await Start(new StartVerificationRequest
            {
                Custom = request.Custom,
                Identity = request.Identity,
                Method = request.Method,
                Reference = request.Reference
            }, cancellationToken);
            return result as StartSmsVerificationResponse;
        }

        /// <inheritdoc />
        public async Task<StartSmsVerificationResponse> StartSms(string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            var result = await Start(new StartVerificationRequest
            {
                Identity = Identity.Number(phoneNumber),
                Method = VerificationMethodEx.Sms
            }, cancellationToken);
            return result as StartSmsVerificationResponse;
        }

        /// <inheritdoc />
        public async Task<StartFlashCallVerificationResponse> StartFlashCall(StartFlashCallVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await Start(new StartVerificationRequest
            {
                Custom = request.Custom,
                Identity = request.Identity,
                Method = request.Method,
                Reference = request.Reference,
                FlashCallOptions = request.FlashCallOptions
            }, cancellationToken);
            return result as StartFlashCallVerificationResponse;
        }

        /// <inheritdoc />
        public async Task<StartPhoneCallVerificationResponse> StartPhoneCall(StartPhoneCallVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await Start(new StartVerificationRequest
            {
                Custom = request.Custom,
                Identity = request.Identity,
                Method = request.Method,
                Reference = request.Reference
            }, cancellationToken);
            return result as StartPhoneCallVerificationResponse;
        }

        /// <inheritdoc />
        public async Task<StartDataVerificationResponse> StartSeamless(StartDataVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await Start(new StartVerificationRequest
            {
                Custom = request.Custom,
                Identity = request.Identity,
                Method = request.Method,
                Reference = request.Reference
            }, cancellationToken);
            return result as StartDataVerificationResponse;
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
                        cancellationToken),
                SmsVerificationReportRequest smsVerificationRequest => _http
                    .Send<SmsVerificationReportRequest, IVerificationReportResponse>(
                        uri, HttpMethod.Put,
                        smsVerificationRequest,
                        cancellationToken),
                PhoneCallVerificationReportRequest phoneRequest => _http
                    .Send<PhoneCallVerificationReportRequest, IVerificationReportResponse>(uri, HttpMethod.Put,
                        phoneRequest,
                        cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(request))
            };
        }
    }
}
