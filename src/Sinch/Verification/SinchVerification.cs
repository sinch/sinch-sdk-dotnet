using System;
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
        Task<StartCalloutVerificationResponse> StartCallout(StartCalloutVerificationRequest request,
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
        /// </summary>
        /// <param name="endpoint">For type number use a E.164-compatible phone number.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportSmsVerificationResponse> ReportSmsByIdentity(string endpoint, ReportSmsVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Report the received verification code to verify it,
        ///     using the identity of the user (in most cases, the phone number).
        /// </summary>
        /// <param name="endpoint">For type number use a E.164-compatible phone number.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportFlashCallVerificationResponse> ReportFlashCallByIdentity(string endpoint,
            ReportFlashCallVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Report the received verification code to verify it,
        ///     using the identity of the user (in most cases, the phone number).
        /// </summary>
        /// <param name="endpoint">For type number use a E.164-compatible phone number.</param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportCalloutVerificationResponse> ReportCalloutByIdentity(string endpoint,
            ReportCalloutVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Report the received verification code to verify it, using the Verification ID of the Verification request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportSmsVerificationResponse> ReportSmsById(string id, ReportSmsVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Report the received verification code to verify it, using the Verification ID of the Verification request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportFlashCallVerificationResponse> ReportFlashCallById(string id,
            ReportFlashCallVerificationRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Report the received verification code to verify it, using the Verification ID of the Verification request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ReportCalloutVerificationResponse> ReportCalloutById(string id,
            ReportCalloutVerificationRequest request,
            CancellationToken cancellationToken = default);
    }

    internal class SinchVerification : ISinchVerification
    {
        private readonly Uri _baseAddress;
        private readonly IHttp _http;
        private readonly ILoggerAdapter<SinchVerification>? _logger;

        public SinchVerification(ILoggerAdapter<SinchVerification>? logger, Uri baseAddress, IHttp http)
        {
            _logger = logger;
            _baseAddress = baseAddress;
            _http = http;
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
            return result as StartSmsVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(StartSmsVerificationResponse)} result is null.");
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
            return result as StartSmsVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(StartSmsVerificationResponse)} result is null.");
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
            return result as StartFlashCallVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(StartFlashCallVerificationResponse)} result is null.");
        }

        /// <inheritdoc />
        public async Task<StartCalloutVerificationResponse> StartCallout(StartCalloutVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await Start(new StartVerificationRequest
            {
                Custom = request.Custom,
                Identity = request.Identity,
                Method = request.Method,
                Reference = request.Reference
            }, cancellationToken);
            return result as StartCalloutVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(StartCalloutVerificationResponse)} result is null.");
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
            return result as StartDataVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(StartDataVerificationResponse)} result is null.");
        }


        private Task<IVerificationReportResponse> ReportIdentity(string endpoint, VerifyReportRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/number/{endpoint}");
            _logger?.LogDebug("Reporting the the code with identity...");

            return Report(request, uri, cancellationToken);
        }

        public async Task<ReportSmsVerificationResponse> ReportSmsByIdentity(string endpoint,
            ReportSmsVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await ReportIdentity(endpoint, request, cancellationToken);
            return result as ReportSmsVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(ReportSmsVerificationResponse)} result is null.");
        }

        public async Task<ReportFlashCallVerificationResponse> ReportFlashCallByIdentity(string endpoint,
            ReportFlashCallVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await ReportIdentity(endpoint, request, cancellationToken);
            return result as ReportFlashCallVerificationResponse ??
                   throw new InvalidOperationException(
                       $"{nameof(ReportFlashCallVerificationResponse)} result is null.");
        }

        public async Task<ReportCalloutVerificationResponse> ReportCalloutByIdentity(string endpoint,
            ReportCalloutVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await ReportIdentity(endpoint, request, cancellationToken);
            return result as ReportCalloutVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(ReportCalloutVerificationResponse)} result is null.");
        }

        private Task<IVerificationReportResponse> ReportId(string id, VerifyReportRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger?.LogDebug("Reporting the the code with id...");
            var uri = new Uri(_baseAddress, $"verification/v1/verifications/id/{id}");

            return Report(request, uri, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<ReportSmsVerificationResponse> ReportSmsById(string id, ReportSmsVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await ReportId(id, request, cancellationToken);
            return result as ReportSmsVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(ReportSmsVerificationResponse)} result is null.");
        }

        /// <inheritdoc />
        public async Task<ReportFlashCallVerificationResponse> ReportFlashCallById(string id,
            ReportFlashCallVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await ReportId(id, request, cancellationToken);
            return result as ReportFlashCallVerificationResponse ??
                   throw new InvalidOperationException(
                       $"{nameof(ReportFlashCallVerificationResponse)} result is null.");
        }

        /// <inheritdoc />
        public async Task<ReportCalloutVerificationResponse> ReportCalloutById(string id,
            ReportCalloutVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await ReportId(id, request, cancellationToken);
            return result as ReportCalloutVerificationResponse ??
                   throw new InvalidOperationException($"{nameof(ReportCalloutVerificationResponse)} result is null.");
        }

        private Task<IStartVerificationResponse> Start(StartVerificationRequest request,
            CancellationToken cancellationToken = default)
        {
            var uri = new Uri(_baseAddress, "verification/v1/verifications");
            _logger?.LogDebug("Starting verification...");
            return _http.Send<StartVerificationRequest, IStartVerificationResponse>(uri, HttpMethod.Post, request,
                cancellationToken);
        }

        private Task<IVerificationReportResponse> Report(VerifyReportRequest request,
            Uri uri, CancellationToken cancellationToken)
        {
            return request switch
            {
                ReportFlashCallVerificationRequest flashCallVerificationReportRequest =>
                    _http.Send<ReportFlashCallVerificationRequest, IVerificationReportResponse>(uri, HttpMethod.Put,
                        flashCallVerificationReportRequest,
                        cancellationToken),
                ReportSmsVerificationRequest smsVerificationRequest => _http
                    .Send<ReportSmsVerificationRequest, IVerificationReportResponse>(
                        uri, HttpMethod.Put,
                        smsVerificationRequest,
                        cancellationToken),
                ReportCalloutVerificationRequest phoneRequest => _http
                    .Send<ReportCalloutVerificationRequest, IVerificationReportResponse>(uri, HttpMethod.Put,
                        phoneRequest,
                        cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(request))
            };
        }
    }
}
